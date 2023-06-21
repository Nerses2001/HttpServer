using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HttpServer.Server.HttpRequest;
using Newtonsoft.Json;

namespace HttpServer.Server
{
    sealed class AppServer
    {
        private string _baseUrl;
        private int _port;
        private HttpListener _listener;

        public AppServer(string baseUrl, int port)
        {
            this._baseUrl = baseUrl;
            this._port = port;
            _listener = new HttpListener();

        }


        public void StartServer(bool turnOn)
        {
          
            string url = BuildUrl();
            SetupListener(url, turnOn);

            while (turnOn)
            {
                var context = _listener.GetContext();
                var request = context.Request;
                var response = context.Response;

                Console.WriteLine("Received request: {0} {1}", request.HttpMethod, request.Url);

                if(request.HttpMethod == "GET")
                {
                    HandleGetRequest(request, response);

                }
                else
                {
                    HandleUnsupportedMethod(request, response);

                }
            }


        }

        private string BuildUrl() { return this._baseUrl + this._port + "/"; }

        private void SetupListener(string url, bool turnOn) 
        {
            _listener.Prefixes.Add(url);
            if(turnOn)
            {
                _listener.Start();
                Console.WriteLine("Server started with " + url);

            }
            else
            {
                _listener.Stop();
                Console.WriteLine("Server stopped " + url);
                return;
            }
        }

        private void HandleGetRequest(HttpListenerRequest request, HttpListenerResponse response)
        {
            string number = GetNumberFromUrl(request.Url.AbsolutePath);

            if (!string.IsNullOrEmpty(number) && int.TryParse(number, out int parsedNumber) && parsedNumber >= 1 && parsedNumber <= 100)
            {
                SendSuccessResponse(response, parsedNumber);
            }
            else
            {
                SendNotFoundResponse(response);
            }
        }

        private string GetNumberFromUrl(string url)
        {
            return Regex.Match(url, @"^/(\d+)$").Groups[1].Value;
        }

        private void SendSuccessResponse(HttpListenerResponse response, int parsedNumber)
        {
            GetAlbums get = new GetAlbums("https://jsonplaceholder.typicode.com/", "albums");
            var responseObject = get.GetData();

            string jsonResponse = JsonConvert.SerializeObject(responseObject);
            byte[] buffer = Encoding.UTF8.GetBytes(jsonResponse);

            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusDescription = "OK";
            response.ContentType = "text/plain";
            response.ContentLength64 = buffer.Length;

            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();

            Console.WriteLine("Sent response with status code: {0}", response.StatusCode);
        }

        private void SendNotFoundResponse(HttpListenerResponse response)
        {
            string responseContent = "404 Not Found";
            byte[] buffer = Encoding.UTF8.GetBytes(responseContent);

            response.StatusCode = (int)HttpStatusCode.NotFound;
            response.StatusDescription = "Not Found";
            response.ContentType = "text/plain";
            response.ContentLength64 = buffer.Length;

            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();

            Console.WriteLine("Sent response with status code: {0}", response.StatusCode);
        }

        private void HandleUnsupportedMethod(HttpListenerRequest request, HttpListenerResponse response)
        {
            string responseContent = "405 Method Not Allowed";
            byte[] buffer = Encoding.UTF8.GetBytes(responseContent);

            response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
            response.StatusDescription = "Method Not Allowed";
            response.ContentType = "text/plain";
            response.ContentLength64 = buffer.Length;
        }


    }




}
