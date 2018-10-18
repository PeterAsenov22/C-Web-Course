namespace SIS.Framework.Controllers
{
    using ActionResults;
    using ActionResults.Contracts;
    using HTTP.Responses.Contracts;
    using HTTP.Requests.Contracts;
    using Services;
    using Services.Implementations;
    using System.Runtime.CompilerServices;
    using Utilities;
    using Views;

    public abstract class Controller
    {
        private const string AuthCookieHeader = ".auth-cookie";

        protected Controller()
        {
            this.ViewBag = new ViewBag();
            this.UserCookieService = new UserCookieService();
        }

        public IHttpRequest Request { get; set; }

        public IHttpResponse Response { get; set; }

        protected ViewBag ViewBag { get; }

        protected IUserCookieService UserCookieService { get; set; }

        protected IViewable View(
            [CallerMemberName] string actionName = "",
            string layoutName = "")
        {
            string controllerName = ControllerUtilities.GetControllerName(this);
            string viewPath = ControllerUtilities.GetViewPath(controllerName, actionName);

            if (layoutName == string.Empty)
            {
                layoutName = MvcContext.Get.DefaultLayoutViewName;
            }

            string layoutPath = ControllerUtilities.GetLayoutPath(layoutName);
            View view = new View(viewPath, layoutPath, this.ViewBag.Data);

            this.ViewBag.Clear();
            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl)
        {
            return new RedirectResult(redirectUrl);
        }

        protected bool IsAuthenticated()
        {
            if (this.Request.Cookies.ContainsCookie(AuthCookieHeader))
            {
                return true;
            }

            return false;
        }

        protected string User
        {
            get
            {
                if (!this.IsAuthenticated())
                {
                    return null;
                }

                var cookie = this.Request.Cookies.GetCookie(AuthCookieHeader);
                var cookieContent = cookie.Value;
                var userName = this.UserCookieService.GetUserData(cookieContent);
                return userName;
            }
        }

        protected void LogoutUser()
        {
            var cookie = this.Request.Cookies.GetCookie(AuthCookieHeader);
            cookie.Delete();
            this.Response.Cookies.Add(cookie);
        }
    }
}
