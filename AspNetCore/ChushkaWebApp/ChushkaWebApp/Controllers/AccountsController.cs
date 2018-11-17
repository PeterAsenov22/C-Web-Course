namespace ChushkaWebApp.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using ChushkaWebApp.Models;
    using ViewModels.Accounts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authorization;

    public class AccountsController : Controller
    {
        private SignInManager<ChushkaUser> signInManager;

        public AccountsController(SignInManager<ChushkaUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = this.signInManager.UserManager.Users.FirstOrDefault(u => u.UserName == model.Username);
            try
            {
                this.signInManager.SignInAsync(user, false).Wait();
            }
            catch (Exception)
            {
                return this.View(model);
            }

            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ChushkaUser()
            {
                Email = model.Email,
                FullName = model.FullName,
                UserName = model.Username
            };

            var result = this.signInManager.UserManager.CreateAsync(user, model.Password).Result;

            if (this.signInManager.UserManager.Users.Count() == 1)
            {
                var roleResult = this.signInManager.UserManager.AddToRoleAsync(user, "Administrator").Result;
                if (roleResult.Errors.Any())
                {
                    return this.View();
                }
            }

            if (result.Succeeded)
            {
                return this.RedirectToAction("Login");
            }

            return View();
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.signInManager.SignOutAsync().Wait();
            return this.RedirectToAction("Index", "Home");
        }
    }
}
