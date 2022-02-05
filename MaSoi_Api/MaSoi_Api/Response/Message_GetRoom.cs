using MaSoi_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Response
{
    public class Player
    {
        public string Tk { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public bool Boss { get; set; }
        public bool Status { get; set; }
        public string BaiId { get; set; }
    }

    public class Message_GetRoom
    {
        private int Status;
        private string Notification;
        private Room RoomInfo;
        private List<Player> Players;

        public Message_GetRoom(int status, string notification, Room roomInfo, List<Player> players)
        {
            Status = status;
            Notification = notification;
            RoomInfo = roomInfo;
            Players = players;
        }

        public int Status1 { get => Status; set => Status = value; }
        public string Notification1 { get => Notification; set => Notification = value; }
        public Room RoomInfo1 { get => RoomInfo; set => RoomInfo = value; }
        public List<Player> Players1 { get => Players; set => Players = value; }
    }
}
