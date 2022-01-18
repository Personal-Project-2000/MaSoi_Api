using MaSoi_Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class HistoryController : Controller
    {
        private HistoryService _historyService;
        private PlayerService _playerService;
        private StoryService _storyService;
        private UserService _userService;
        private BaiService _baiService;

        public HistoryController(BaiService baiService, HistoryService historyService, UserService userService, PlayerService playerService, StoryService storyService)
        {
            _historyService = historyService;
            _playerService = playerService;
            _storyService = storyService;
            _userService = userService;
            _baiService = baiService;
        }

        [HttpGet]
        [Route("HistoryList_Get")]
        public IActionResult GetHistoryList(string Tk)
        {
            var historyList = _playerService.GetAllHistory(Tk);

            List<Response.HistoryList> historyLists = new List<Response.HistoryList>();
            foreach(var item in historyList)
            {
                Response.HistoryList history = new Response.HistoryList();
                history.HistoryId = item.HistoryId;
                history.Sl = _playerService.PlayerCount(item.HistoryId);
                history.Time = _historyService.GetTime(item.HistoryId);
                history.StartTime = _historyService.GetStartTime(item.HistoryId);
                history.Win = _playerService.Win(item.HistoryId, Tk);

                historyLists.Add(history);
            }

            return Ok(new Response.Message_HistoryList(1, "Lấy dữ liệu thành công", historyLists));
        }

        [HttpGet]
        [Route("History_Get")]
        public IActionResult GetHistory(string HistoryId)
        {
            var playerList = _playerService.GetAllPlayer(HistoryId);

            List<Response.Player_History> players = new List<Response.Player_History>();
            foreach (var item in playerList)
            {
                var p = _userService.GetPlayer(item.Tk);
                var p1 = _playerService.GetPlayer(item.Tk);

                Response.Player_History player = new Response.Player_History();
                player.Tk = p.Tk;
                player.Name = p.FullName;
                player.Img = p.Img;
                player.BaiName = _baiService.BaiName(p1.BaiId);
                player.Win = p1.Win;

                players.Add(player);
            }

            var storyList = _storyService.GetStory(HistoryId);

            return Ok(new Response.Message_History(1, "Lấy dữ liệu thành công", players, storyList));
        }
    }
}
