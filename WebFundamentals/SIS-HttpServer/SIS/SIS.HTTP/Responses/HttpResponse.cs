namespace SIS.HTTP.Responses
{
    using Cookies;
    using Cookies.Contracts;
    using Common;
    using Extensions;
    using Enums;
    using Headers;
    using Headers.Contracts;
    using Contracts;
    using System.Linq;
    using System.Text;

    public class HttpResponse : IHttpResponse
    {
        public HttpResponse() { }

        public HttpResponse(HttpResponseStatusCode statusCode)
        {
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
            this.Content = new byte[0];
            this.StatusCode = statusCode;
        }

        public HttpResponseStatusCode StatusCode { get; }

        public IHttpHeaderCollection Headers { get; private set; }

        public IHttpCookieCollection Cookies { get; private set;  }

        public byte[] Content { get; set; }

        public void AddHeader(HttpHeader header)
        {
            this.Headers.Add(header);
        }

        public void AddCookie(HttpCookie cookie)
        {
            this.Cookies.Add(cookie);
        }

        public byte[] GetBytes()
        {
            var resultAsBytes = Encoding.UTF8.GetBytes(this.ToString());
            var response = resultAsBytes.Concat(this.Content).ToArray();
            return response;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result
                .AppendLine($"{GlobalConstants.HttpOneProtocolFragment} {this.StatusCode.GetResponseLine()}")
                .AppendLine($"{this.Headers}");

            if (this.Cookies.HasCookies())
            {
                foreach (var httpCookie in this.Cookies)
                {
                    result.AppendLine($"{HttpHeader.SetCookie}: {httpCookie}");
                }
            }

            result.AppendLine();

            return result.ToString();
        }
    }
}
