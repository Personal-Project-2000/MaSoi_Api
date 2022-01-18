using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Response
{
    public class HistoryList
    {
        public string HistoryId { get; set; }
        public int Sl { get; set; }
        public string StartTime { get; set; }
        public string Time { get; set; }
        public bool Win { get; set; }
    }

    public class Message_HistoryList
    {
        private int Status;
        private string Notification;
        private List<HistoryList> HistoryLists;

        public Message_HistoryList(int status, string notification, List<HistoryList> historyLists)
        {
            Status = status;
            Notification = notification;
            HistoryLists = historyLists;
        }

        public int Status1 { get => Status; set => Status = value; }
        public string Notification1 { get => Notification; set => Notification = value; }
        public List<HistoryList> HistoryLists1 { get => HistoryLists; set => HistoryLists = value; }
    }
}
