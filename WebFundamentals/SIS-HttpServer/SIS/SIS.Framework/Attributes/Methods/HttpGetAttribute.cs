namespace SIS.Framework.Attributes.Methods
{
    using HTTP.Enums;

    public class HttpGetAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod)
        {
            if (requestMethod.ToUpper() == HttpRequestMethod.Get.ToString().ToUpper())
            {
                return true;
            }

            return false;
        }
    }
}
