using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MaSoi_Api.Models
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Number { get; set; }
        public string Pass { get; set; }
        public int VoteTime { get; set; }
        public int AdvocateTime { get; set; }
        public int Sl { get; set; }
        public bool Format { get; set; }
        public bool Status { get; set; }
    }
}
