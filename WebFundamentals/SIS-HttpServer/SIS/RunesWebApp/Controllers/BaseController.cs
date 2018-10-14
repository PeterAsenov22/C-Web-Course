namespace RunesWebApp.Controllers
{
    using Data;
    using SIS.Framework.Controllers;

    public abstract class BaseController : Controller
    {
        protected RunesContext Db { get; set; }

        protected BaseController()
        {
            this.Db = new RunesContext();
        }
    }
}
