using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MaSoi_Api.Models
{
    public class RoomDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string RoomId { get; set; }
        public string Tk { get; set; }
        public bool Status { get; set; }
        public bool Boss { get; set; }
    }
}
