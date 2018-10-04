namespace CameraBazaar.Web.Models.UserViewModels
{
    using Services.Models.Camera;
    using System.Collections.Generic;

    public class UserProfileViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int CamerasInStock { get; set; }

        public int CamerasOutOfStock { get; set; }

        public IEnumerable<CameraModel> Cameras { get; set; }
    }
}
