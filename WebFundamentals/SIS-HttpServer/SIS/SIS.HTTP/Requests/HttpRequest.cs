namespace SIS.HTTP.Requests
{
    using Cookies;
    using Cookies.Contracts;
    using Common;
    using Enums;
    using Exceptions;
    using Headers;
    using Headers.Contracts;
    using Contracts;
    using Sessions.Contracts;
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
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, object> FormData { get; }

        public Dictionary<string, object> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public IHttpCookieCollection Cookies { get; }

        public IHttpSession Session { get; set; }

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

            this.ParseRequestMethod(requestLine.First());
            this.ParseRequestUrl(requestLine[1]);
            this.ParseRequestPath(this.Url);
            this.ParseQuery();

            this.ParseRequestHeaders(requestLines.Skip(1).ToArray());
            this.ParseCookies();
            this.ParseFormData(requestLines.Last());
        }

        private bool IsValidRequestLine(string[] requestLine)
        {
            return requestLine.Length == 3 && requestLine[2].ToLower() == "http/1.1";
        }

        private void ParseRequestMethod(string method)
        {
            HttpRequestMethod parsedMethod;

            if (!Enum.TryParse(method, true, out parsedMethod))
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            this.RequestMethod = parsedMethod;
        }

        private void ParseRequestUrl(string url)
        {
            CoreValidator.ThrowIfNullOrEmpty(url, nameof(url));
            this.Url = url;
        }

        private void ParseRequestPath(string url)
        {
            this.Path = url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        private void ParseQuery()
        {
            if (!this.Url.Contains('?'))
            {
                return;
            }

            var query = this.Url
                .Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Take(1)
                .ToString();

            this.ParseData(query, this.QueryData);
        }

        private void ParseRequestHeaders(string[] requestHeadersLines)
        {
            if (!requestHeadersLines.Any())
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

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

        private void ParseCookies()
        {
            if (this.Headers.ContainsHeader(GlobalConstants.CookieRequestHeaderName))
            {
                var cookiesRaw = this.Headers.GetHeader(GlobalConstants.CookieRequestHeaderName).Value;
                var cookies = cookiesRaw.Split(new [] {"; "}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var cookie in cookies)
                {
                    var cookieKeyValuePair = cookie.Split(new[] { '=' }, 2);
                    if (cookieKeyValuePair.Length != 2)
                    {
                        BadRequestException.ThrowFromInvalidRequest();
                    }

                    this.Cookies.Add(new HttpCookie(cookieKeyValuePair[0], cookieKeyValuePair[1]));
                }
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

                dictionary[dataKey] = dataValue;
            }
        }
    }
}
