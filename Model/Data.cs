using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//https://jsonplaceholder.typicode.com/albums
namespace HttpServer.Model
{
    class Data
    {
        public int UserId { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }

        public Data(int userId, string id, string title){ 
        
            this.UserId = userId;
            this.Id = id;   
            this.Title = title;
        }


    }
}
