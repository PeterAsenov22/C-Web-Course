namespace RunesWebApp.Controllers
{
    using SIS.HTTP.Responses.Contracts;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            return View();
        }
    }
}
