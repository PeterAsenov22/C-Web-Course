namespace SIS.HTTP.Cookies
{
    using Contracts;
    using Common;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly IDictionary<string, HttpCookie> cookies;

        public HttpCookieCollection()
        {
            this.cookies = new Dictionary<string, HttpCookie>();
        }

        public void Add(HttpCookie cookie)
        {
            CoreValidator.ThrowIfNull(cookie, nameof(cookie));

            if (!string.IsNullOrEmpty(cookie.Key) &&
                !string.IsNullOrEmpty(cookie.Value) &&
                !this.ContainsCookie(cookie.Key))
            {
                this.cookies.Add(cookie.Key, cookie);
            }
        }

        public bool ContainsCookie(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));

            return this.cookies.ContainsKey(key);
        }

        public HttpCookie GetCookie(string key)
        {
            if (this.ContainsCookie(key))
            {
                return this.cookies[key];
            }

            return null;
        }

        public bool HasCookies()
        {
            return this.cookies.Any();
        }

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            foreach (var cookie in this.cookies)
            {
                yield return cookie.Value;
            }
        }

        public override string ToString()
        {
            return string.Join("; ", this.cookies.Values);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
