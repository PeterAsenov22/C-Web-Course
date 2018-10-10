using System.IO;
using SIS.HTTP.Enums;
using SIS.HTTP.Responses;

namespace SIS.WebServer
{
    using HTTP.Cookies;
    using HTTP.Common;
    using HTTP.Requests;
    using HTTP.Requests.Contracts;
    using HTTP.Responses.Contracts;
    using HTTP.Sessions;
    using Results;
    using Routing;
    using System;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class ConnectionHandler
    {
        private readonly Socket client;

        private readonly ServerRoutingTable serverRoutingTable;

        private const string RootDirectoryRelativePath = "../../../";

        public ConnectionHandler(Socket client, ServerRoutingTable serverRoutingTable)
        {
            this.client = client;
            this.serverRoutingTable = serverRoutingTable;
        }

        private async Task<IHttpRequest> ReadRequest()
        {
            var result = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);

            while (true)
            {
                int numberOfBytesRead = await this.client.ReceiveAsync(data, SocketFlags.None);

                if (numberOfBytesRead == 0)
                {
                    break;
                }

                var bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);
                result.Append(bytesAsString);

                if (numberOfBytesRead < 1023)
                {
                    break;
                }
            }

            if (result.Length == 0)
            {
                return null;
            }

            return new HttpRequest(result.ToString());
        }

        private IHttpResponse HandleRequest(IHttpRequest request)
        {
            var response = this.TryHandleResourceRequest(request);
            if (response != null)
            {
                return response;
            }

            if (!this.serverRoutingTable.Routes.ContainsKey(request.RequestMethod)
                || !this.serverRoutingTable.Routes[request.RequestMethod].ContainsKey(request.Path))
            {
                return new NotFoundResult("404 Page Not Found");
            }

            return this.serverRoutingTable.Routes[request.RequestMethod][request.Path].Invoke(request);
        }

        private IHttpResponse TryHandleResourceRequest(IHttpRequest request)
        {
            var requestPath = request.Path;
            if (requestPath.Contains("."))
            {
                var filePath = $"{RootDirectoryRelativePath}Resources{requestPath}";
                if (!File.Exists(filePath))
                {
                    return new HttpResponse(HttpResponseStatusCode.NotFound);
                }

                var fileContent = File.ReadAllBytes(filePath);

                return new InlineResourceResult(fileContent);
            }

            return null;
        }

        private async Task PrepareResponse(IHttpResponse response)
        {
            var byteSegments = new ArraySegment<byte>(response.GetBytes());

            await this.client.SendAsync(byteSegments, SocketFlags.None);
        }

        private string SetRequestSession(IHttpRequest httpRequest)
        {
            string sessionId;

            if (httpRequest.Cookies.ContainsCookie(HttpSessionStorage.SessionCookieKey))
            {
                var cookie = httpRequest.Cookies.GetCookie(HttpSessionStorage.SessionCookieKey);
                sessionId = cookie.Value;
            }
            else
            {
                sessionId = Guid.NewGuid().ToString();
            }

            httpRequest.Session = HttpSessionStorage.GetSession(sessionId);
            return sessionId;
        }

        private void SetResponseSession(IHttpResponse httpResponse, string sessionId)
        {
            if (sessionId != null)
            {
                var sessionCookie = new HttpCookie(HttpSessionStorage.SessionCookieKey, $"{sessionId}");
                httpResponse.AddCookie(sessionCookie);
            }
        }

        public async Task ProcessRequestAsync()
        {
            var httpRequest = await this.ReadRequest();

            if (httpRequest != null)
            {
                string sessionId = SetRequestSession(httpRequest);

                var httpResponse = this.HandleRequest(httpRequest);

                SetResponseSession(httpResponse, sessionId);

                await this.PrepareResponse(httpResponse);
            }

            this.client.Shutdown(SocketShutdown.Both);
        }
    }
}
