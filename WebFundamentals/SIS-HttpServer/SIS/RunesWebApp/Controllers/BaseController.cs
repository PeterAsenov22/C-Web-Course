namespace RunesWebApp.Controllers
{
    using Data;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public abstract class BaseController
    {
        private const string RootDirectoryRelativePath = "../../../";

        private const string ControllerDefaultName = "Controller";

        private const string ViewsFolderName = "Views";

        private const string TemplatePlaceholder = "@RenderBody";

        protected RunesContext Context { get; set; }

        protected BaseController()
        {
            this.Context = new RunesContext();
        }

        private string GetCurrentControllerName()
        {
            return this.GetType().Name.Replace(ControllerDefaultName, String.Empty);
        }

        protected IHttpResponse View([CallerMemberName] string viewName = "")
        {     
            string layoutPath = $"{RootDirectoryRelativePath}{ViewsFolderName}/Layout.html";
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
            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }
    }
}
