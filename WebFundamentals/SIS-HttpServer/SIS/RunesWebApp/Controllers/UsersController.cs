namespace RunesWebApp.Controllers
{
    using Models;
    using SIS.Framework.ActionResults.Contracts;
    using SIS.Framework.Attributes.Methods;
    using SIS.Framework.Services;
    using SIS.HTTP.Cookies;
    using System;
    using System.Linq;
    using ViewModels;
    using ViewModels.Users;

    public class UsersController : BaseController
    {
        private readonly IHashService hashService;

        public UsersController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return this.RedirectToAction("/users/login");
            }

            var hashedPassword = this.hashService.Hash(model.Password);
            var user = this.Db
                .Users
                .FirstOrDefault(u => u.Username == model.Username && u.HashedPassword == hashedPassword);

            if (user is null)
            {
                return this.RedirectToAction("/users/login");
            }

            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);

            var cookie = new HttpCookie(".auth-cookie", cookieContent, 7);
            this.Response.Cookies.Add(cookie);

            return this.RedirectToAction("/");
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Length < 4)
            {
                return this.View();
                // return new BadRequestResult("<h1>Please provide valid username with length of 4 or more characters.</h1>");
            }

            if (string.IsNullOrWhiteSpace(model.Email) || model.Email.Length < 4)
            {
                return this.View();
                // return new BadRequestResult("<h1>Please provide valid email with length of 4 or more characters.</h1>");
            }

            if (this.Db.Users.Any(x => x.Username == model.Username))
            {
                return this.View();
                // return new BadRequestResult("User with the same name already exists.");
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6)
            {
                return this.View();
                // return new BadRequestResult("Please provide password of length 6 or more.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.View();
                // return new BadRequestResult("Passwords do not match.");
            }

            var hashedPassword = this.hashService.Hash(model.Password);

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                HashedPassword = hashedPassword
            };

            this.Db.Users.Add(user);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception)
            {
                return this.RedirectToAction("/users/register");
            }

            return this.RedirectToAction("/users/login");
        }

        public IActionResult Logout()
        {
            if (!this.IsAuthenticated())
            {
                return this.RedirectToAction("/");
            }

            this.LogoutUser();
            return this.RedirectToAction("/");
        }
    }
}
