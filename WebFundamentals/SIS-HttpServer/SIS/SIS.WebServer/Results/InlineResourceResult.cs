namespace SIS.WebServer.Results
{
    using HTTP.Enums;
    using HTTP.Headers;
    using HTTP.Responses;

    public class InlineResourceResult : HttpResponse
    {
        public InlineResourceResult(byte[] content)
          :base(HttpResponseStatusCode.Ok)
        {
            this.Headers.Add(new HttpHeader(HttpHeader.ContentLength, content.Length.ToString()));
            this.Headers.Add(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            this.Content = content;
        }
    }
}
