namespace SIS.HTTP.Extensions
{
    public static class StringExtensions
    {
        public static string Capitalize(this string word)
        {
            return word[0].ToString().ToUpper() + word.Substring(1).ToLower();
        }
    }
}
