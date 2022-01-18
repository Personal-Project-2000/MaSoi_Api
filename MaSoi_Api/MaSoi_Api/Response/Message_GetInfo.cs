using MaSoi_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Response
{
    public class Message_GetInfo
    {
        private int Status;
        private string Notification;
        private User Acc;

        public Message_GetInfo(int status, string notification, User acc)
        {
            Status = status;
            Notification = notification;
            Acc = acc;
        }

        public int Status1 { get => Status; set => Status = value; }
        public string Notification1 { get => Notification; set => Notification = value; }
        public User Acc1 { get => Acc; set => Acc = value; }
    }
}
