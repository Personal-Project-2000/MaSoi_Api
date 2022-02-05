using MaSoi_Api.Models;
using MaSoi_Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class BaiController : Controller
    {
        private readonly BaiService _baiService;
        private readonly PlayerService _playerService;
        private readonly RoomDetailService _roomDetailService;
        public BaiController(IOptions<MaSoiDatabaseSettings> maSoiDataBaseSetting)
        {
            _baiService = new BaiService(maSoiDataBaseSetting);
            _playerService = new PlayerService(maSoiDataBaseSetting);
            _roomDetailService = new RoomDetailService(maSoiDataBaseSetting);
        }

        [HttpGet]
        [Route("Bai_Get")]
        public async Task<IActionResult> Bai(string baiId, string Tk, string roomId, string historyId)
        {
            var bai = await _baiService.GetBai(baiId);

            if (bai is null)
            {
                return Ok(new Response.Message_GetBai(0, "Lấy dữ liệu thất bại", null));
            }

            var roomDetail = await _roomDetailService.GetPlayer(Tk, roomId);
            if (roomDetail != null)
            {
                roomDetail.BaiId = baiId;
                await _roomDetailService.UpdateAsync(roomDetail);
            }

            var player = await _playerService.GetAsync(Tk, historyId);
            if (player != null)
            {
                player.BaiId = baiId;
                await _playerService.UpdateAsync(player);
            }

            return Ok(new Response.Message_GetBai(1, "Lấy dữ liệu thành công", bai));
        }
    }
}
