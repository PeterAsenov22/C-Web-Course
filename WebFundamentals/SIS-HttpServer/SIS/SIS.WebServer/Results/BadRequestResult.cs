namespace SIS.WebServer.Results
{
    using HTTP.Enums;
    using HTTP.Headers;
    using HTTP.Responses;
    using System.Text;

    public class BadRequestResult : HttpResponse
    {
        public BadRequestResult(string content)
            : base(HttpResponseStatusCode.BadRequest)
        {
            this.Headers.Add(new HttpHeader(HttpHeader.ContentType, "text/html"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}
