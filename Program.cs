using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HttpServer.Server;

namespace HttpServer
{
    class Program
    {
        static void Main()
        {
            string url = "http://localhost:"; 

            var server = new AppServer(url, 8080);

            server.StartServer(true);
          
        }
    }
}
