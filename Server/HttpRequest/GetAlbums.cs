using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HttpServer.Model;
using Newtonsoft.Json;

namespace HttpServer.Server.HttpRequest
{
    class GetAlbums
    {
        private string _baseUrl;
        private string _endUrl;
        private HttpClient _httpClient;


        public GetAlbums(string baseUrl, string endUrl)
        {
            this._baseUrl = baseUrl;
            this._endUrl = endUrl;
            _httpClient = new HttpClient();
        }

        public async Task<List<Data>> GetData()
        {
            Console.WriteLine("GetData");

            try
            {
                var response = await _httpClient.GetAsync(_baseUrl + _endUrl);

                if (response.IsSuccessStatusCode)
                {
                  
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<Data>>(json);

                    Console.WriteLine($"Received data: {data[0].Title}, {data[1].Id}");

                    return data;
                }
                else
                {
                    Console.WriteLine($"HTTP GET request failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return null;

        }
    }
}

