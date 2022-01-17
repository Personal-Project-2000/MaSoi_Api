using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Models
{
    public class MaSoiDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UserCollectionName { get; set; } = null!;
        public string RoomCollectionName { get; set; } = null!;
        public string RoomDetailCollectionName { get; set; } = null!;
        public string HistoryCollectionName { get; set; } = null!;
        public string PlayerCollectionName { get; set; } = null!;
        public string StoryCollectionName { get; set; } = null!;
        public string BaiCollectionName { get; set; } = null!;
        public string BaiRoomCollectionName { get; set; } = null!;
    }
}
