namespace RequestParser
{
    using System;
    using System.Collections.Generic;

    public class Program
    {
        public static void Main(string[] args)
        {
            var registeredPaths = new Dictionary<string, List<string>>();

            Console.Write("Register path: ");
            string input = Console.ReadLine();

            while (input != null && !input.Equals("END"))
            {
                if (!string.IsNullOrEmpty(input))
                {
                    var inputParts = input.Split('/', StringSplitOptions.RemoveEmptyEntries);
                    var path = inputParts[0];
                    var method = inputParts[1];

                    if (!registeredPaths.ContainsKey(path))
                    {
                        registeredPaths[path] = new List<string>();
                    }

                    registeredPaths[path].Add(method);
                }

                Console.Write("Register path: ");
                input = Console.ReadLine();
            }

            Console.Write("Enter request: ");
            var request = Console.ReadLine();
            if (request != null)
            {
                var requestParts = request.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var requestMethod = requestParts[0].ToLower();
                var path = requestParts[1].Substring(1);
                var protocol = requestParts[2];

                if (registeredPaths.ContainsKey(path))
                {
                    var availableMethodsForPath = registeredPaths[path];
                    if (availableMethodsForPath.Contains(requestMethod))
                    {
                        Console.WriteLine($@"
                        {protocol} 200 OK
                        Content-Length: 2
                        Content-Type: text/plain
                        
                        OK");
                    }
                    else
                    {
                        Console.WriteLine($@"
                        {protocol} 404 NotFound
                        Content-Length: 8
                        Content-Type: text/plain
                        
                        NotFound");
                    }
                }
                else
                {
                    Console.WriteLine($@"
                        {protocol} 404 NotFound
                        Content-Length: 8
                        Content-Type: text/plain
                        
                        NotFound");
                }
            }
        }
    }
}
