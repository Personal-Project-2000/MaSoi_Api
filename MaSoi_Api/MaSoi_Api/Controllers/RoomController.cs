using MaSoi_Api.Models;
using MaSoi_Api.Response;
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
    public class RoomController : Controller
    {
        private readonly RoomService _roomService;
        private readonly RoomDetailService _roomDetailService;
        private readonly UserService _userService;
        private readonly HistoryService _historyService;
        private readonly PlayerService _playerService;

        public RoomController(RoomService roomService, RoomDetailService roomDetailService, UserService userService, HistoryService history, PlayerService player) { 
            _roomService = roomService;
            _roomDetailService = roomDetailService;
            _userService = userService;
            _historyService = history;
            _playerService = player;
        }

        [HttpGet]
        [Route("RoomList_Get")]
        public async Task<IActionResult> RoomList()
        {
            var room = await _roomService.GetList();

            return Ok(new Message_RoomList(1, "Lấy dữ liệu thành công", room));
        }

        [HttpPost]
        [Route("CreateRoom_Post")]
        public async Task<IActionResult> CreateRoom(string Tk)
        {
            var newRoom = await _roomService.GetRoomNull();

            if(newRoom is null)
            {
                return Ok(new Response.Message(0, "Server hiện đang quá tải", null));
            }

            newRoom.Sl = 1;
            newRoom.Status = true;
            newRoom.Format = true;
            newRoom.AdvocateTime = 5;
            newRoom.VoteTime = 5;
            newRoom.Pass = "";

            await _roomService.UpdateAsync(newRoom);

            RoomDetail newPlayer = new RoomDetail();
            newPlayer.RoomId = newRoom.Id;
            newPlayer.Tk = Tk;
            newPlayer.Status = true;
            newPlayer.Boss = true;
            newPlayer.BaiId = "";

            await _roomDetailService.InsertPlayer(newPlayer);

            return Ok(new Response.Message(1, "Tạo phòng thành công", newRoom.Id));
        }

        [HttpPost]
        [Route("UpdateRoom_Post")]
        public async Task<IActionResult> UpdateRoom(Request.SettingRoom_Request input)
        {
            var room = await _roomService.GetAsync1(input.Id);

            if (room is null)
            {
                return NotFound();
            }

            room.AdvocateTime = input.AdvocateTime;
            room.VoteTime = input.VoteTime;
            room.Pass = input.Pass;
            room.Format = input.Format;

            await _roomService.UpdateAsync(room);

            return Ok(new Response.Message(1, "Cài đặt phòng thành công", null));
        }

        [HttpPost]
        [Route("JoinRoom_Post")]
        public async Task<IActionResult> JoinRoom(Request.JoinRoom_Request input)
        {
            var room = await _roomService.CheckPass(input.RoomId, input.Pass);

            if (room is null)
            {
                return Ok(new Response.Message(0, "Mật khẩu bị sai", null));
            }

            room.Sl ++;

            await _roomService.UpdateAsync(room);

            RoomDetail newPlayer = new RoomDetail();
            newPlayer.RoomId = room.Id;
            newPlayer.Tk = input.Tk;
            newPlayer.Status = false;
            newPlayer.Boss = false;
            newPlayer.BaiId = "";

            await _roomDetailService.InsertPlayer(newPlayer);

            return Ok(new Response.Message(1, "Vào phòng thành công", null));
        }

        [HttpGet]
        [Route("Room_Get")]
        public async Task<IActionResult> GetRoom(string RoomId) 
        {
            var room = await _roomService.GetAsync1(RoomId);

            if (room is null)
            {
                return NotFound();
            }

            var playerList = _roomDetailService.GetAllPlayer(RoomId);

            List<Response.Player> playerL = new List<Response.Player>();

            foreach (var item in playerList)
            {
                var user = _userService.CheckTk(item.Tk);

                Response.Player player = new Response.Player();
                player.Tk = item.Tk;
                player.Img = user.Img;
                player.Name = user.FullName;
                player.Status = item.Status;
                player.Boss = item.Boss;
                player.BaiId = "";

                playerL.Add(player);
            }

            return Ok(new Response.Message_GetRoom(1, "Lấy dữ liệu thành công", room, playerL));
        }

        [HttpPost]
        [Route("ExitRoom_Post")]
        public async Task<IActionResult> ExitRoom(string Tk, string RoomId)
        {
            var room = await _roomService.GetAsync1(RoomId);

            if (room is null)
            {
                return NotFound();
            }

            room.Sl--;

            await _roomService.UpdateAsync(room);

            if(room.Sl == 0)
            {
                await _roomDetailService.RemoveAsync(RoomId, Tk);

                return Ok(new Response.Message(1, "Xóa dữ liệu thành công", null));
            }

            //Trao quyền chủ phòng có người kế tiếp
            var player = await _roomDetailService.GetPlayer(Tk, RoomId);

            if (player.Boss)
            {
                await _roomDetailService.RemoveAsync(RoomId, Tk);

                var playerHeader = await _roomDetailService.GetPlayerHeader(RoomId);
                playerHeader.Boss = true;
                playerHeader.Status = true;
                await _roomDetailService.UpdateAsync(playerHeader);
            }
            else
            {
                await _roomDetailService.RemoveAsync(RoomId, Tk);
            }

            return Ok(new Response.Message(1, "Xóa dữ liệu thành công", null));
        }

        [HttpPost]
        [Route("Ready_Post")]
        public async Task<IActionResult> Ready(string Tk, string RoomId)
        {
            var player = await _roomDetailService.GetPlayer(Tk, RoomId);

            if (player is null)
            {
                return NotFound();
            }

            player.Status = !player.Status;

            await _roomDetailService.UpdateAsync(player);

            return Ok(new Response.Message(1, "Sẳn sàng thành công", null));
        }

        [HttpPost]
        [Route("Start_Post")]
        public async Task<IActionResult> Start(string RoomId, int Amount)
        {
            var players =  _roomDetailService.GetAllPlayer(RoomId);

            if (players is null)
            {
                return NotFound();
            }

            if (players.Count() < Amount)
            {
                return Ok(new Response.Message(2, "Chưa đủ " + Amount + " người chơi", null));
            }

            //Kiểm tra tất cả người chơi đã sẳn sàng và số lượng người chơi phải trên 5
            if (players.Where(x => x.Status == true).Count() == players.Count())
            {
                var room = await _roomService.GetAsync1(RoomId);

                room.Status = false;

                await _roomService.UpdateAsync(room);

                var history = new History();
                history.StartTime = DateTime.Now+"";
                history.Time = "";

                await _historyService.CreateAsync(history);

                foreach(var item in players)
                {
                    var player = new Models.Player();
                    player.Tk = item.Tk;
                    player.HistoryId = history.Id;
                    player.BaiId = "";
                    player.Win = false;

                    await _playerService.CreateAsync(player);
                }

                return Ok(new Response.Message(1, "Tất cả đã sẳn sàng", history.Id));
            }

            return Ok(new Response.Message(0, "Có vài người chơi chưa sẳn sàng", null));
        }
    }
}
