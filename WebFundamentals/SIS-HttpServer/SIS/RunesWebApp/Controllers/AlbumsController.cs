namespace RunesWebApp.Controllers
{
    using Models;
    using SIS.Framework.ActionResults.Contracts;
    using SIS.Framework.Attributes.Methods;
    using System.Linq;
    using ViewModels.Albums;

    public class AlbumsController : BaseController
    {
        public IActionResult All()
        {
            if (!this.IsAuthenticated())
            {
                return this.RedirectToAction("/users/login");
            }

            var albums = this.Db
                .Albums
                .Select(a => new
                {
                    Id = a.Id,
                    Name = a.Name
                })
                .ToArray();

            string albumsHtml = string.Empty;

            if (albums.Length == 0)
            {
                albumsHtml = @"<p class=""text-warning"">There are currently no albums.</p>";
            }
            else
            {
                foreach (var album in albums)
                {
                    albumsHtml += $@"<div><a class=""font-weight-bold"" href=""/albums/details?id={album.Id}"">{album.Name}</a></div>";
                }
            }

            this.ViewBag["Albums"] = albumsHtml;

            return View("All", "AuthLayout");
        }

        public IActionResult Create()
        {
            if (!this.IsAuthenticated())
            {
                return this.RedirectToAction("/users/login");
            }

            return View("Create", "AuthLayout");
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel model)
        {
            if (!this.IsAuthenticated())
            {
                return this.RedirectToAction("/users/login");
            }

            var album = new Album
            {
                Name = model.Name,
                Cover = model.Cover
            };

            this.Db.Albums.Add(album);
            this.Db.SaveChanges();

            return this.RedirectToAction("/albums/all");
        }

        public IActionResult Details(string id)
        {
            if (!this.IsAuthenticated())
            {
                return this.RedirectToAction("/users/login");
            }

            if (id is null)
            {
                return this.RedirectToAction("/albums/all");
            }

            var album = this.Db
                .Albums
                .FirstOrDefault(a => a.Id == id);

            if (album is null)
            {
                return this.RedirectToAction("/albums/all");
            }

            string tracksHtml = string.Empty;
            if (!album.Tracks.Any())
            {
                tracksHtml += @"<p class=""text-warning"">There are currently no tracks.</p>";
            }
            else
            {
                var tracks = album.Tracks.ToArray();
                tracksHtml += "<ul>";
                for (int i = 0; i < tracks.Length; i++)
                {
                    var currTrack = tracks[i];
                    tracksHtml += $@"<li> <span class=""font-weight-bold"">{i + 1}</span>. <a class=""font-italic"" href=""/tracks/details?albumId={album.Id}&trackId={currTrack.Id}"">{currTrack.Name}</a></li>";
                }

                tracksHtml += "</ul>";
            }

            this.ViewBag["Id"] = album.Id;
            this.ViewBag["Name"] = album.Name;
            this.ViewBag["Cover"] = album.Cover;
            this.ViewBag["Price"] = album.Price.ToString("F2");
            this.ViewBag["Tracks"] = tracksHtml;

            return View("Details", "AuthLayout");
        }
    }
}
