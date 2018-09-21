namespace URLDecode
{
    using System;
    using System.Net;

    public class Program
    {
        public static void Main()
        {          
            while (true)
            {
                string encodedUrl;

                do
                {
                    Console.Write("Enter encoded URL: ");
                    encodedUrl = Console.ReadLine();

                    if (string.IsNullOrEmpty(encodedUrl))
                    {
                        Console.WriteLine("URL must not be empty string!");
                    }

                } while (string.IsNullOrEmpty(encodedUrl));

                string decodedUrl = WebUtility.UrlDecode(encodedUrl);
                Console.WriteLine("Decoded URL: " + decodedUrl);
            }
        }
    }
}
