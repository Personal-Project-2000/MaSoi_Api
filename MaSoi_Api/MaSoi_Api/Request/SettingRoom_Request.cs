using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Request
{
    public class SettingRoom_Request
    {
        public string RoomId { get; set; }
        public string Pass { get; set; }
        public bool Format { get; set; }
        public int AdvocateTime { get; set; }
        public int VoteTime { get; set; }
    }
}
