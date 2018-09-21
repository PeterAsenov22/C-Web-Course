namespace ValidateURL
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;

    public class Program
    {
        static void Main()
        {
            while (true)
            {
                string encodedUrl;

                do
                {
                    Console.Write("Enter URL: ");
                    encodedUrl = Console.ReadLine();

                    if (string.IsNullOrEmpty(encodedUrl))
                    {
                        Console.WriteLine("URL must not be empty string!");
                    }

                } while (string.IsNullOrEmpty(encodedUrl));

                string decodedUrl = WebUtility.UrlDecode(encodedUrl);

                var mainRegex = new Regex("^(http|https):\\/\\/([A-Za-z0-9-]+\\.[A-Za-z0-9-]+)(:(\\d+))?(\\/?[^\\s]+)?$");
                if (mainRegex.IsMatch(decodedUrl))
                {
                    var match = mainRegex.Match(decodedUrl);

                    var protocol = match.Groups[1].Value;               
                    var host = match.Groups[2].Value;              

                    int port;
                    var portMatch = match.Groups[4].Value;
                    if (string.IsNullOrEmpty(portMatch))
                    {
                        port = protocol.Equals("http") ? 80 : 443;
                    }
                    else
                    {
                        port = int.Parse(portMatch);
                    }

                    if ((port.Equals(443) && protocol.Equals("http")) || (port.Equals(80) && protocol.Equals("https")))
                    {
                        Console.WriteLine("Invalid URL");
                    }
                    else
                    {
                        Console.WriteLine("Protocol: " + protocol);
                        Console.WriteLine("Host: " + host);
                        Console.WriteLine("Port: " + port);

                        string path;
                        var pathMatch = match.Groups[5].Value;
                        if (string.IsNullOrEmpty(pathMatch))
                        {
                            path = "/";
                            Console.WriteLine("Path: " + path);
                        }
                        else if (pathMatch.Contains("?") || pathMatch.Contains("#"))
                        {
                            var pathRegex = new Regex("^(.*?)(\\?([^#]*))?(#(.*))?$");
                            var currentMatch = pathRegex.Match(pathMatch);

                            path = currentMatch.Groups[1].Value;
                            Console.WriteLine("Path: " + path);

                            var query = currentMatch.Groups[3].Value;
                            if (!string.IsNullOrEmpty(query))
                            {
                                Console.WriteLine("Query: " + query);
                            }

                            var fragment = currentMatch.Groups[5].Value;
                            if (!string.IsNullOrEmpty(fragment))
                            {
                                Console.WriteLine("Fragment: " + fragment);
                            }
                        }
                        else
                        {
                            path = pathMatch;
                            Console.WriteLine("Path: " + path);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid URL");
                }
            }
        }
    }
}
