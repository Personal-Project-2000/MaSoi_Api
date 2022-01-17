using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MaSoi_Api.Models
{
    public class Player
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Tk { get; set; }
        public string HistoryId { get; set; }
        public bool Win { get; set; }
        public string BaiId { get; set; }
    }
}
