namespace CameraBazaar.Web.Controllers
{
    using Data.Models;
    using Models.UserViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("user")]
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ICameraService cameras;

        public UserController(ICameraService cameras, UserManager<User> userManager)
        {
            this.cameras = cameras;
            this.userManager = userManager;
        }

        [Authorize]
        [Route("profile")]
        public async Task<IActionResult> Profile()
        {
            string userId = this.userManager.GetUserId(User);
            User user = await this.userManager.FindByIdAsync(userId);

            var userCameras = this.cameras.GetUserCameras(userId).ToList();

            var userProfileViewModel = new UserProfileViewModel
            {
                Id = userId,
                Username = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                CamerasInStock = userCameras.Count(c => c.Quantity > 0),
                CamerasOutOfStock = userCameras.Count(c => c.Quantity == 0),
                Cameras = userCameras
            };

            return View(userProfileViewModel);
        }

        [Authorize]
        [Route("profile/edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await this.userManager.FindByIdAsync(id);

            var profileEditViewModel = new ProfileEditViewModel
            {
                Email = user.Email
            };

            return View(profileEditViewModel);
        }

        [Authorize]
        [HttpPost]
        [Route("profile/edit/{id}")]
        public async Task<IActionResult> Edit(ProfileEditViewModel model, string id)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = await this.userManager.FindByIdAsync(id);

            IdentityResult changePass = await this.userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);
            if (!changePass.Succeeded)
            {
                return View(model);
            }

            string emailToken = await this.userManager.GenerateChangeEmailTokenAsync(user, model.Email);
            IdentityResult changeEmail = await this.userManager.ChangeEmailAsync(user, model.Email, emailToken);
            if (!changeEmail.Succeeded)
            {
                return View(model);
            }

            return RedirectToAction(nameof(Profile));
        }
    }
}
