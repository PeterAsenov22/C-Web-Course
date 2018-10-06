namespace RunesWebApp.Controllers
{
    using Models;
    using Services;
    using Services.Implementations;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Responses.Contracts;
    using SIS.HTTP.Requests.Contracts; 
    using SIS.WebServer.Results;
    using System;
    using System.Linq;

    public class UsersController : BaseController
    {
        private readonly IHashService hashService;

        public UsersController()
        {
            this.hashService = new HashService();
        }

        public IHttpResponse Login()
        {
            return View();
        }

        public IHttpResponse Register()
        {
            return View();
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString();
            var password = request.FormData["password"].ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return new RedirectResult("/users/login");
            }

            var hashedPassword = this.hashService.Hash(password);
            var user = this.Context
                .Users
                .FirstOrDefault(u => u.Username == username && u.HashedPassword == hashedPassword);

            if (user is null)
            {
                return new RedirectResult("/users/login");
            }

            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);

            var response = new RedirectResult("/");
            var cookie = new HttpCookie(".auth-cookie", cookieContent, 7);
            response.Cookies.Add(cookie);

            return response;
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString();
            var email = request.FormData["email"].ToString();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmPassword"].ToString();

            if (string.IsNullOrWhiteSpace(username) || username.Length < 4)
            {
                return new BadRequestResult("<h1>Please provide valid username with length of 4 or more characters.</h1>");
            }

            if (string.IsNullOrWhiteSpace(email) || email.Length < 4)
            {
                return new BadRequestResult("<h1>Please provide valid email with length of 4 or more characters.</h1>");
            }

            if (this.Context.Users.Any(x => x.Username == username))
            {
                return new BadRequestResult("User with the same name already exists.");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return new BadRequestResult("Please provide password of length 6 or more.");
            }

            if (password != confirmPassword)
            {
                return new BadRequestResult("Passwords do not match.");
            }

            var hashedPassword = this.hashService.Hash(password);

            var user = new User
            {
                Username = username,
                Email = email,
                HashedPassword = hashedPassword
            };

            this.Context.Users.Add(user);

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception)
            {
                return new RedirectResult("/users/register");
            }

            return new RedirectResult("/users/login");
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/");
            }

            return this.LogoutUser(request);
        }
    }
}
