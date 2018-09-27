namespace SIS.HTTP.Requests
{
    using Common;
    using Enums;
    using Exceptions;
    using Headers;
    using Headers.Interfaces;
    using Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            this.FormData = new Dictionary<string, object>();
            this.QueryData = new Dictionary<string, object>();
            this.Headers = new HttpHeaderCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        private void ParseRequest(string requestText)
        {
            var requestLines = requestText.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None);

            if (!requestLines.Any())
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            var requestLine = requestLines.First().Split(
                new[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);

            if (!IsValidRequestLine(requestLine))
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            this.RequestMethod = this.ParseMethod(requestLine.First());
            this.Url = requestLine[1];
            this.Path = this.ParsePath(this.Url);
            this.ParseQuery();

            this.ParseHeaders(requestLines.Skip(1).ToArray());
            this.ParseFormData(requestLines.Last());
        }

        private bool IsValidRequestLine(string[] requestLine)
        {
            return requestLine.Length == 3 && requestLine[2].ToLower() == "http/1.1";
        }

        private HttpRequestMethod ParseMethod(string method)
        {
            HttpRequestMethod parsedMethod;

            if (!Enum.TryParse(method, true, out parsedMethod))
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            return parsedMethod;
        }

        private string ParsePath(string url)
            => url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)[0];

        private void ParseQuery()
        {
            if (!this.Url.Contains('?'))
            {
                return;
            }

            var query = this.Url
                .Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries)
                .Last();

            this.ParseData(query, this.QueryData);
        }

        private void ParseHeaders(string[] requestHeadersLines)
        {
            var emptyLineAfterHeadersIndex = Array.IndexOf(requestHeadersLines, string.Empty);

            for (int i = 0; i < emptyLineAfterHeadersIndex; i++)
            {
                var currentLine = requestHeadersLines[i];
                var headerParts = currentLine.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);

                if (headerParts.Length != 2)
                {
                    BadRequestException.ThrowFromInvalidRequest();
                }

                var headerKey = headerParts[0];
                var headerValue = headerParts[1].Trim();

                var header = new HttpHeader(headerKey, headerValue);

                this.Headers.Add(header);
            }

            if (!this.Headers.ContainsHeader(GlobalConstants.HostHeaderKey))
            {
                BadRequestException.ThrowFromInvalidRequest();
            }
        }

        private void ParseFormData(string formDataLine)
        {
            if (this.RequestMethod == HttpRequestMethod.Get)
            {
                return;
            }

            this.ParseData(formDataLine, this.FormData);
        }

        private void ParseData(string data, Dictionary<string, object> dictionary)
        {
            if (!data.Contains('='))
            {
                return;
            }

            var dataPairs = data.Split(new[] { '&' });

            foreach (var dataPair in dataPairs)
            {
                var dataKvp = dataPair.Split(new[] { '=' });

                if (dataKvp.Length != 2)
                {
                    return;
                }

                var dataKey = WebUtility.UrlDecode(dataKvp[0]);
                var dataValue = WebUtility.UrlDecode(dataKvp[1]);

                dictionary.Add(dataKey, dataValue);
            }
        }
    }
}
