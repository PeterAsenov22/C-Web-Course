namespace SIS.Framework
{
    using HTTP.Enums;
    using HTTP.Requests.Contracts;
    using HTTP.Responses;
    using HTTP.Responses.Contracts;
    using WebServer.Api;
    using WebServer.Results;
    using WebServer.Routing;
    using System.IO;

    public class HttpHandler : IHttpHandler
    {
        private ServerRoutingTable serverRoutingTable;

        private const string RootDirectoryRelativePath = "../../../";

        public HttpHandler(ServerRoutingTable serverRoutingTable)
        {
            this.serverRoutingTable = serverRoutingTable;
        }

        public IHttpResponse Handle(IHttpRequest request)
        {
            var response = this.TryHandleResourceRequest(request);
            if (response != null)
            {
                return response;
            }

            if (!this.serverRoutingTable.Routes.ContainsKey(request.RequestMethod)
                || !this.serverRoutingTable.Routes[request.RequestMethod].ContainsKey(request.Path))
            {
                return new NotFoundResult("404 Page Not Found");
            }

            return this.serverRoutingTable.Routes[request.RequestMethod][request.Path].Invoke(request);
        }

        private IHttpResponse TryHandleResourceRequest(IHttpRequest request)
        {
            var requestPath = request.Path;
            if (requestPath.Contains("."))
            {
                var filePath = $"{RootDirectoryRelativePath}Resources{requestPath}";
                if (!File.Exists(filePath))
                {
                    return new HttpResponse(HttpResponseStatusCode.NotFound);
                }

                var fileContent = File.ReadAllBytes(filePath);

                return new InlineResourceResult(fileContent);
            }

            return null;
        }
    }
}
