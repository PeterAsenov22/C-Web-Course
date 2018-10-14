namespace RunesWebApp.Controllers
{
    using Models;
    using SIS.Framework.ActionResults.Contracts;
    using SIS.Framework.Attributes;
    using SIS.Framework.Attributes.Methods;
    using System.Linq;
    using System.Collections.Generic;

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
            var viewBag = new Dictionary<string, string>();
            viewBag.Add("AlbumId", albumId);

            return View("Create", viewBag, "AuthLayout");
        }

        [HttpPost]
        [Route("/create")]
        public IActionResult CreatePost()
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
            var name = this.Request.FormData["name"].ToString();
            var link = this.Request.FormData["link"].ToString();
            var price = this.Request.FormData["price"].ToString();

            var track = new Track
            {
                Name = name,
                Link = link,
                Price = decimal.Parse(price)
            };

            var album = this.Db.Albums.FirstOrDefault(a => a.Id == albumId);
            if (album is null)
            {
                return this.RedirectToAction("/albums/all");
            }

            album.Tracks.Add(track);
            this.Db.SaveChanges();

            return this.RedirectToAction($"/albums/details?id={album.Id}");
        }

        public IActionResult Details()
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

            var albumId = this.Request.QueryData["albumId"].ToString();
            var trackId = this.Request.QueryData["trackId"].ToString();

            var track = this.Db.Tracks.FirstOrDefault(t => t.Id == trackId);
            if (track is null)
            {
                return this.RedirectToAction($"/albums/details?id={albumId}");
            }

            var viewBag = new Dictionary<string, string>();
            viewBag.Add("Name", track.Name);
            viewBag.Add("Price", track.Price.ToString("F2"));
            viewBag.Add("Link", track.Link);
            viewBag.Add("AlbumId", albumId);

            return View("Details", viewBag, "AuthLayout");
        }
    }
}
