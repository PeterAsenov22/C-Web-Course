namespace RunesWebApp.Controllers
{
    using System.Collections.Generic;
    using SIS.Framework.ActionResults.Contracts;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            if (this.IsAuthenticated())
            {
                var username = this.User;
                var viewBag = new Dictionary<string, string>();
                viewBag.Add("Username", username);

                string authViewName = "IndexAuthenticated";
                return View(authViewName, viewBag, "AuthLayout");
            }
            
            return View();
        }
    }
}
