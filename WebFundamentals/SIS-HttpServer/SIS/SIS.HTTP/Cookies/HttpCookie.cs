﻿namespace SIS.HTTP.Cookies
{
    using System;

    public class HttpCookie
    {
        private const int HttpCookieDefaultExpirationDays = 3;

        public HttpCookie(string key, string value, int expiresInDays = HttpCookieDefaultExpirationDays)
        {
            this.Key = key;
            this.Value = value;
            this.IsNew = true;
            this.Expires = DateTime.UtcNow.AddDays(expiresInDays);
        }

        public HttpCookie(string key, string value, bool isNew, int expiresInDays = HttpCookieDefaultExpirationDays)
            : this(key, value, expiresInDays)
        {
            this.IsNew = isNew;
        }

        public string Key { get; }

        public string Value { get; }

        public DateTime Expires { get; private set; }

        public bool IsNew { get; }

        public void Delete()
        {
            this.Expires = DateTime.UtcNow.AddDays(-1);
        }

        public override string ToString()
        {
            return $@"{this.Key}={this.Value}; Path=/; Expires={this.Expires:R}; HttpOnly;";
        }
    }
}
