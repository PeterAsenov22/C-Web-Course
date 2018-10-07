namespace RunesWebApp.Controllers
{
    using Models;
    using SIS.HTTP.Responses.Contracts;
    using SIS.HTTP.Requests.Contracts;
    using SIS.WebServer.Results;
    using System.Collections.Generic;
    using System.Linq;  

    public class AlbumsController : BaseController
    {
        public IHttpResponse All(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            var albums = this.Context
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
                albumsHtml = "<p>There are currently no albums.</p>";
            }
            else
            {
                foreach (var album in albums)
                {
                    albumsHtml += $@"<div><a href=""/albums/details?id={album.Id}"">{album.Name}</a></div>";
                }
            }

            var viewBag = new Dictionary<string, string>();
            viewBag.Add("Albums", albumsHtml);

            return View("All", viewBag, true);
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            return View("Create", null, true);
        }

        public IHttpResponse CreatePost(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            var albumName = request.FormData["name"].ToString();
            var cover = request.FormData["cover"].ToString();

            var album = new Album
            {
                Name = albumName,
                Cover = cover
            };

            this.Context.Albums.Add(album);
            this.Context.SaveChanges();

            return new RedirectResult("/albums/all");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            if (!request.QueryData.ContainsKey("id"))
            {
                return new RedirectResult("/albums/all");
            }

            var albumId = request.QueryData["id"].ToString();
            if (albumId is null)
            {
                return new RedirectResult("/albums/all");
            }

            var album = this.Context
                .Albums
                .FirstOrDefault(a => a.Id == albumId);

            if (album is null)
            {
                return new RedirectResult("/albums/all");
            }

            string tracksHtml = string.Empty;
            if (!album.Tracks.Any())
            {
                tracksHtml += "<p>There are currently no tracks.</p>";
            }
            else
            {
                var tracks = album.Tracks.ToArray();
                tracksHtml += "<ul>";
                for (int i = 0; i < tracks.Length; i++)
                {
                    var currTrack = tracks[i];
                    tracksHtml += $@"<li> {i+1}. <a href=""/tracks/details?albumId={album.Id}&trackId={currTrack.Id}"">{currTrack.Name}</a></li>";
                }

                tracksHtml += "</ul>";
            }

            var viewBag = new Dictionary<string, string>();
            viewBag.Add("Id", album.Id);
            viewBag.Add("Name", album.Name);
            viewBag.Add("Cover", album.Cover);
            viewBag.Add("Price", album.Price.ToString("F2"));
            viewBag.Add("Tracks", tracksHtml);

            return View("Details", viewBag, true);
        }
    }
}
