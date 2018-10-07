namespace SIS.WebServer.Results
{
    using HTTP.Enums;
    using HTTP.Headers;
    using HTTP.Responses;
    using System.Text;

    public class NotFoundResult : HttpResponse
    {
        public NotFoundResult(string message)
            : base(HttpResponseStatusCode.NotFound)
        {
            this.Headers.Add(new HttpHeader("Content-Type", "text/html"));
            this.Content = Encoding.UTF8.GetBytes($"<h1>{message}</h1>");
        }
    }
}
