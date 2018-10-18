namespace RunesWebApp.Controllers
{
    using SIS.Framework.ActionResults.Contracts;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            if (this.IsAuthenticated())
            {
                var username = this.User;
                this.ViewBag["Username"] = username;

                string authViewName = "IndexAuthenticated";
                return View(authViewName, "AuthLayout");
            }
            
            return View();
        }
    }
}
