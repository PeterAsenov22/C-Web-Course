namespace SIS.Framework.Attributes.Methods
{
    using HTTP.Enums;

    public class HttpDeleteAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod)
        {
            if (requestMethod.ToUpper() == HttpRequestMethod.Delete.ToString().ToUpper())
            {
                return true;
            }

            return false;
        }
    }
}
