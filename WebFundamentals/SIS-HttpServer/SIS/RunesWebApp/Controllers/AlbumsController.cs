namespace RunesWebApp.Controllers
{
    using Models;
    using SIS.Framework.ActionResults.Contracts;
    using SIS.Framework.Attributes;
    using SIS.Framework.Attributes.Methods;
    using System.Linq;  

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
        [Route("/create")]
        public IActionResult CreatePost()
        {
            if (!this.IsAuthenticated())
            {
                return this.RedirectToAction("/users/login");
            }

            var albumName = this.Request.FormData["name"].ToString();
            var cover = this.Request.FormData["cover"].ToString();

            var album = new Album
            {
                Name = albumName,
                Cover = cover
            };

            this.Db.Albums.Add(album);
            this.Db.SaveChanges();

            return this.RedirectToAction("/albums/all");
        }

        public IActionResult Details()
        {
            if (!this.IsAuthenticated())
            {
                return this.RedirectToAction("/users/login");
            }

            if (!this.Request.QueryData.ContainsKey("id"))
            {
                return this.RedirectToAction("/albums/all");
            }

            var albumId = this.Request.QueryData["id"].ToString();
            if (albumId is null)
            {
                return this.RedirectToAction("/albums/all");
            }

            var album = this.Db
                .Albums
                .FirstOrDefault(a => a.Id == albumId);

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
