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

        public RoomController(RoomService roomService, RoomDetailService roomDetailService, UserService userService) { 
            _roomService = roomService;
            _roomDetailService = roomDetailService;
            _userService = userService;
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
            var player =  _roomDetailService.GetAllPlayer(RoomId);

            if (player is null)
            {
                return NotFound();
            }

            if (player.Count() < Amount)
            {
                return Ok(new Response.Message(2, "Chưa đủ " + Amount + " người chơi", null));
            }

            //Kiểm tra tất cả người chơi đã sẳn sàng và số lượng người chơi phải trên 5
            if (player.Where(x => x.Status == true).Count() == player.Count())
            {
                var room = await _roomService.GetAsync1(RoomId);

                room.Status = false;

                await _roomService.UpdateAsync(room);

                return Ok(new Response.Message(1, "Tất cả đã sẳn sàng", null));
            }

            return Ok(new Response.Message(0, "Có vài người chơi chưa sẳn sàng", null));
        }
    }
}
