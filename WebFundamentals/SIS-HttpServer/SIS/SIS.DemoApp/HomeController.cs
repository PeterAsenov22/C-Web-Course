namespace SIS.DemoApp
{
    using HTTP.Enums;
    using HTTP.Responses.Contracts;
    using WebServer.Results;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            string content = "<h1>Hello, world!";

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }
    }
}
