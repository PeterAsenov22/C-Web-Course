namespace SIS.HTTP.Requests.Interfaces
{
    using System.Collections.Generic;
    using Enums;
    using Headers.Interfaces;

    public interface IHttpRequest
    {
        string Path { get; }

        string Url { get; }

        Dictionary<string, object> FormData { get; }

        Dictionary<string, object> QueryData { get; }

        IHttpHeaderCollection Headers { get; }

        HttpRequestMethod RequestMethod { get; }
    }
}
