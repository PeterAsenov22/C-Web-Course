namespace RunesWebApp.Controllers
{
    using Models;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System.Linq;
    using System.Collections.Generic;

    public class TracksController : BaseController
    {
        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            if (!request.QueryData.ContainsKey("albumId"))
            {
                return new RedirectResult("/albums/all");
            }

            var albumId = request.QueryData["albumId"].ToString();
            var viewBag = new Dictionary<string, string>();
            viewBag.Add("AlbumId", albumId);

            return View("Create", viewBag, true);
        }

        public IHttpResponse CreatePost(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            if (!request.QueryData.ContainsKey("albumId"))
            {
                return new RedirectResult("/albums/all");
            }

            var albumId = request.QueryData["albumId"].ToString();
            var name = request.FormData["name"].ToString();
            var link = request.FormData["link"].ToString();
            var price = request.FormData["price"].ToString();

            var track = new Track
            {
                Name = name,
                Link = link,
                Price = decimal.Parse(price)
            };

            var album = this.Context.Albums.FirstOrDefault(a => a.Id == albumId);
            if (album is null)
            {
                return new RedirectResult("/albums/all");
            }

            album.Tracks.Add(track);
            this.Context.SaveChanges();

            return new RedirectResult($"/albums/details?id={album.Id}");
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            if (!request.QueryData.ContainsKey("albumId"))
            {
                return new RedirectResult("/albums/all");
            }

            if (!request.QueryData.ContainsKey("trackId"))
            {
                return new RedirectResult("/albums/all");
            }

            var albumId = request.QueryData["albumId"].ToString();
            var trackId = request.QueryData["trackId"].ToString();

            var track = this.Context.Tracks.FirstOrDefault(t => t.Id == trackId);
            if (track is null)
            {
                return new RedirectResult($"/albums/details?id={albumId}");
            }

            var viewBag = new Dictionary<string, string>();
            viewBag.Add("Name", track.Name);
            viewBag.Add("Price", track.Price.ToString("F2"));
            viewBag.Add("Link", track.Link);
            viewBag.Add("AlbumId", albumId);

            return View("Details", viewBag, true);
        }
    }
}
