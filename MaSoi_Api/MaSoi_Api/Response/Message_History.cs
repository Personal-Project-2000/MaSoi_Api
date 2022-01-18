using MaSoi_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Response
{
    public class Player_History
    {
        public string Tk { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
        public string BaiName { get; set; }
        public bool Win { get; set; }
    }

    public class Message_History
    {
        private int Status;
        private string Notification;
        private List<Player_History> Players;
        private List<Story> Stories;

        public Message_History(int status, string notification, List<Player_History> players, List<Story> stories)
        {
            Status = status;
            Notification = notification;
            Players = players;
            Stories = stories;
        }

        public int Status1 { get => Status; set => Status = value; }
        public string Notification1 { get => Notification; set => Notification = value; }
        public List<Player_History> Players1 { get => Players; set => Players = value; }
        public List<Story> Stories1 { get => Stories; set => Stories = value; }
    }
}
