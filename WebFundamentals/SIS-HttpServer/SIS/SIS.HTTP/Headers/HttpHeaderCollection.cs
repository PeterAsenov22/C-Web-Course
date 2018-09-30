namespace SIS.HTTP.Headers
{
    using Contracts;
    using Common;
    using System;
    using System.Collections.Generic;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly IDictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            this.headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, nameof(header));

            if (!string.IsNullOrEmpty(header.Key) &&
                !string.IsNullOrEmpty(header.Value) &&
                !this.ContainsHeader(header.Key))
            {
                this.headers.Add(header.Key, header);
            }
        }

        public bool ContainsHeader(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));

            return this.headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            if (this.ContainsHeader(key))
            {
                return this.headers[key];
            }

            return null;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, this.headers.Values);
        }
    }
}
