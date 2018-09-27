namespace SIS.HTTP.Responses.Interfaces
{
    using Enums;
    using Headers;
    using Headers.Interfaces;

    public interface IHttpResponse
    {
        HttpResponseStatusCode StatusCode { get; }

        IHttpHeaderCollection Headers { get; }

        byte[] Content { get; set; }

        void AddHeader(HttpHeader header);

        byte[] GetBytes();
    }
}
