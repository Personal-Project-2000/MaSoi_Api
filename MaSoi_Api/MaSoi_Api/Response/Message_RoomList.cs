using MaSoi_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Response
{
    public class Message_RoomList
    {
        private int Status;
        private string Notification;
        private List<Room> RoomInfo;

        public Message_RoomList(int status, string notification, List<Room> roomInfo)
        {
            Status = status;
            Notification = notification;
            RoomInfo = roomInfo;
        }

        public int Status1 { get => Status; set => Status = value; }
        public string Notification1 { get => Notification; set => Notification = value; }
        public List<Room> RoomInfo1 { get => RoomInfo; set => RoomInfo = value; }
    }
}
