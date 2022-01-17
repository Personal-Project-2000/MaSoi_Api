using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Response
{
    public class Message
    {
        private int Status;
        private string Notification;
        private string Id;

        public Message(int status, string notification, string id)
        {
            Status = status;
            Notification = notification;
            Id = id;
        }

        public int Status1 { get => Status; set => Status = value; }
        public string Notification1 { get => Notification; set => Notification = value; }
        public string Id1 { get => Id; set => Id = value; }
    }
}
