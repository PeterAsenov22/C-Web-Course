namespace EventuresWebApp.Web.Controllers
{
    using AutoMapper;
    using System;
    using System.Linq;
    using Models;
    using ViewModels.Accounts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class AccountsController : Controller
    {
        private readonly SignInManager<EventuresUser> signInManager;
        private readonly IMapper mapper;

        public AccountsController(SignInManager<EventuresUser> signInManager, IMapper mapper)
        {
            this.signInManager = signInManager;
            this.mapper = mapper;
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
                this.signInManager.SignInAsync(user, model.RememberMe).Wait();
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

            var user = this.mapper.Map<EventuresUser>(model);

            var result = this.signInManager.UserManager.CreateAsync(user, model.Password).Result;

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
