namespace RunesWebApp.Controllers
{
    using System.Collections.Generic;
    using SIS.HTTP.Responses.Contracts;
    using SIS.HTTP.Requests.Contracts;

    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                var username = this.GetUsername(request);
                var viewBag = new Dictionary<string, string>();
                viewBag.Add("Username", username);
                return View("IndexAuthenticated", viewBag, true);
            }
            
            return View("IndexAnonymous");
        }
    }
}
