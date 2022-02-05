using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Response
{
    public class Message_Adress
    {
        private int Status;
        private string Notification;
        private string Port;
        private string Ip;

        public Message_Adress(int status, string notification, string port, string ip)
        {
            Status = status;
            Notification = notification;
            Port = port;
            Ip = ip;
        }

        public int Status1 { get => Status; set => Status = value; }
        public string Notification1 { get => Notification; set => Notification = value; }
        public string Port1 { get => Port; set => Port = value; }
        public string Ip1 { get => Ip; set => Ip = value; }
    }
}
