namespace RunesWebApp.Controllers
{
    using Data;
    using Services;
    using Services.Implementations;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;

    public abstract class BaseController
    {
        private const string RootDirectoryRelativePath = "../../../";

        private const string ControllerDefaultName = "Controller";

        private const string ViewsFolderName = "Views";

        private const string TemplatePlaceholder = "@RenderBody";

        private const string AuthCookieHeader = ".auth-cookie";

        protected RunesContext Context { get; set; }

        protected IUserCookieService UserCookieService { get; set; }

        protected BaseController()
        {
            this.Context = new RunesContext();
            this.UserCookieService = new UserCookieService();
        }

        private string GetCurrentControllerName()
        {
            return this.GetType().Name.Replace(ControllerDefaultName, String.Empty);
        }

        protected IHttpResponse View([CallerMemberName] string viewName = "", IDictionary<string, string> viewBag = null, bool isAuthenticated = false)
        {
            string layoutPath = 
                isAuthenticated 
                    ? $"{RootDirectoryRelativePath}{ViewsFolderName}/AuthLayout.html" 
                    : $"{RootDirectoryRelativePath}{ViewsFolderName}/Layout.html";

            string filePath =
                $"{RootDirectoryRelativePath}{ViewsFolderName}/{this.GetCurrentControllerName()}/{viewName}.html";

            string layout = File.ReadAllText(layoutPath);
            string content;

            if (!File.Exists(filePath))
            {
                content = layout.Replace(TemplatePlaceholder, $"<h1>View {viewName} not found.</h1>");
                return new BadRequestResult(content);
            }

            content = layout.Replace(TemplatePlaceholder, File.ReadAllText(filePath));
            if (viewBag != null)
            {
                foreach (var item in viewBag)
                {
                    content = content.Replace("@Model." + item.Key, item.Value);
                }
            }

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }

        protected bool IsAuthenticated(IHttpRequest request)
        {
            if (request.Cookies.ContainsCookie(AuthCookieHeader))
            {
                return true;
            }

            return false;
        }

        protected string GetUsername(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return null;
            }

            var cookie = request.Cookies.GetCookie(AuthCookieHeader);
            var cookieContent = cookie.Value;
            var userName = this.UserCookieService.GetUserData(cookieContent);
            return userName;
        }

        protected IHttpResponse LogoutUser(IHttpRequest request)
        {
            var cookie = request.Cookies.GetCookie(AuthCookieHeader);
            cookie.Delete();
            var response = new RedirectResult("/");
            response.Cookies.Add(cookie);
            return response;
        }
    }
}
