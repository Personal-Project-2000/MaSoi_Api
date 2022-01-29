using MaSoi_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Response
{
    public class Message_GetBai
    {
        private int Status;
        private string Notification;
        private Bai BaiInfo;

        public Message_GetBai(int status, string notification, Bai baiInfo)
        {
            Status = status;
            Notification = notification;
            BaiInfo = baiInfo;
        }

        public int Status1 { get => Status; set => Status = value; }
        public string Notification1 { get => Notification; set => Notification = value; }
        public Bai BaiInfo1 { get => BaiInfo; set => BaiInfo = value; }
    }
}
