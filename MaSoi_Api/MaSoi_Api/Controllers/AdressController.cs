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
    public class AdressController : Controller
    {
        private readonly AdressService _adressService;
        public AdressController(IOptions<MaSoiDatabaseSettings> maSoiDataBaseSetting)
        {
            _adressService = new AdressService(maSoiDataBaseSetting);
        }

        [HttpGet]
        [Route("Adress_Get")]
        public async Task<IActionResult> Adress()
        {
            var adress = await _adressService.GetAsync();

            if(adress is null)
            {
                return Ok(new Response.Message_Adress(0, "Lấy dữ liệu thất bại", "", ""));
            }

            return Ok(new Response.Message_Adress(1, "Lấy dữ liệu thành công", adress.Port, adress.Ip));
        }
    }
}
