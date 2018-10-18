namespace RunesWebApp.Controllers
{
    using Models;
    using SIS.Framework.ActionResults.Contracts;
    using SIS.Framework.Attributes.Methods;
    using System.Linq;
    using ViewModels.Tracks;

    public class TracksController : BaseController
    {
        public IActionResult Create()
        {
            if (!this.IsAuthenticated())
            {
                return this.RedirectToAction("/users/login");
            }

            if (!this.Request.QueryData.ContainsKey("albumId"))
            {
                return this.RedirectToAction("/albums/all");
            }

            var albumId = this.Request.QueryData["albumId"].ToString();
            this.ViewBag["AlbumId"] = albumId;

            return View("Create", "AuthLayout");
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel model)
        {
            if (!this.IsAuthenticated())
            {
                return this.RedirectToAction("/users/login");
            }

            if (!this.Request.QueryData.ContainsKey("albumId"))
            {
                return this.RedirectToAction("/albums/all");
            }

            var track = new Track
            {
                Name = model.Name,
                Link = model.Link,
                Price = model.Price
            };

            var album = this.Db.Albums.FirstOrDefault(a => a.Id == model.AlbumId);
            if (album is null)
            {
                return this.RedirectToAction("/albums/all");
            }

            album.Tracks.Add(track);
            this.Db.SaveChanges();

            return this.RedirectToAction($"/albums/details?id={album.Id}");
        }

        public IActionResult Details(string albumId, string trackId)
        {
            if (!this.IsAuthenticated())
            {
                return this.RedirectToAction("/users/login");
            }

            if (!this.Request.QueryData.ContainsKey("albumId"))
            {
                return this.RedirectToAction("/albums/all");
            }

            if (!this.Request.QueryData.ContainsKey("trackId"))
            {
                return this.RedirectToAction("/albums/all");
            }

            var track = this.Db.Tracks.FirstOrDefault(t => t.Id == trackId);
            if (track is null)
            {
                return this.RedirectToAction($"/albums/details?id={albumId}");
            }

            this.ViewBag["Name"] = track.Name;
            this.ViewBag["Price"] = track.Price.ToString("F2");
            this.ViewBag["Link"] = track.Link;
            this.ViewBag["AlbumId"] = albumId;

            return View("Details", "AuthLayout");
        }
    }
}
