namespace SIS.Framework.Attributes.Methods
{
    using HTTP.Enums;

    public class HttpPostAttribute : HttpMethodAttribute
    {
        public override bool IsValid(string requestMethod)
        {
            if (requestMethod.ToUpper() == HttpRequestMethod.Post.ToString().ToUpper())
            {
                return true;
            }

            return false;
        }
    }
}
