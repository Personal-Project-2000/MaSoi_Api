using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MaSoi_Api.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Tk { get; set; }
        public string FullName { get; set; }
        public string Pass { get; set; }
        public string Img { get; set; }
        public string ImgBack { get; set; }
        public string Language { get; set; }
        public bool Background { get; set; }
    }
}
