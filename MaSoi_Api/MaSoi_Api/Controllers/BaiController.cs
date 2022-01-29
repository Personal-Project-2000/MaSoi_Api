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
        public BaiController(IOptions<MaSoiDatabaseSettings> maSoiDataBaseSetting)
        {
            _baiService = new BaiService(maSoiDataBaseSetting);
        }

        [HttpGet]
        [Route("Bai_Get")]
        public async Task<IActionResult> Bai(string baiId)
        {
            var bai = await _baiService.GetBai(baiId);

            if (bai is null)
            {
                return Ok(new Response.Message_GetBai(0, "Lấy dữ liệu thất bại", null));
            }

            return Ok(new Response.Message_GetBai(1, "Lấy dữ liệu thành công", bai));
        }
    }
}
