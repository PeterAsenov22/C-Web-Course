namespace RunesWebApp.Controllers
{
    using Models;
    using SIS.Framework.ActionResults.Contracts;
    using SIS.Framework.Attributes;
    using SIS.Framework.Attributes.Methods;
    using SIS.Framework.Services;
    using SIS.Framework.Services.Implementations;
    using SIS.HTTP.Cookies;
    using System;
    using System.Linq;

    public class UsersController : BaseController
    {
        private readonly IHashService hashService;

        public UsersController()
        {
            this.hashService = new HashService();
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
        [Route("/login")]
        public IActionResult DoLogin()
        {
            var username = this.Request.FormData["username"].ToString();
            var password = this.Request.FormData["password"].ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return this.RedirectToAction("/users/login");
            }

            var hashedPassword = this.hashService.Hash(password);
            var user = this.Db
                .Users
                .FirstOrDefault(u => u.Username == username && u.HashedPassword == hashedPassword);

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
        [Route("/register")]
        public IActionResult DoRegister()
        {
            var username = this.Request.FormData["username"].ToString();
            var email = this.Request.FormData["email"].ToString();
            var password = this.Request.FormData["password"].ToString();
            var confirmPassword = this.Request.FormData["confirmPassword"].ToString();

            if (string.IsNullOrWhiteSpace(username) || username.Length < 4)
            {
                return this.View();
                // return new BadRequestResult("<h1>Please provide valid username with length of 4 or more characters.</h1>");
            }

            if (string.IsNullOrWhiteSpace(email) || email.Length < 4)
            {
                return this.View();
                // return new BadRequestResult("<h1>Please provide valid email with length of 4 or more characters.</h1>");
            }

            if (this.Db.Users.Any(x => x.Username == username))
            {
                return this.View();
                // return new BadRequestResult("User with the same name already exists.");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return this.View();
                // return new BadRequestResult("Please provide password of length 6 or more.");
            }

            if (password != confirmPassword)
            {
                return this.View();
                // return new BadRequestResult("Passwords do not match.");
            }

            var hashedPassword = this.hashService.Hash(password);

            var user = new User
            {
                Username = username,
                Email = email,
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
