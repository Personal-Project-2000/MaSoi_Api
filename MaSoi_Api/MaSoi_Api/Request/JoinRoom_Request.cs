using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Request
{
    public class JoinRoom_Request
    {
        public string RoomId { get; set; }
        public string Tk { get; set; }
        public string Pass { get; set; }
    }
}
