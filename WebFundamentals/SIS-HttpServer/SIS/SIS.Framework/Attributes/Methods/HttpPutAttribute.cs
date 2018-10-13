﻿namespace SIS.Framework.Attributes.Methods
{
    using HTTP.Enums;

    public class HttpPutAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod)
        {
            if (requestMethod.ToUpper() == HttpRequestMethod.Put.ToString().ToUpper())
            {
                return true;
            }

            return false;
        }
    }
}
