using MaSoi_Api.Models;
using MaSoi_Api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MaSoi_Api.Controllers
{
    [ApiController]
    [Route("api/")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly IHostingEnvironment _HostEnvironment;
        private readonly RoomService _roomService;
        private readonly RoomDetailService _roomDetailService;

        public UserController(IOptions<MaSoiDatabaseSettings> maSoiDataBaseSetting, IHostingEnvironment hostingEnvironment)
        {
            _userService = new UserService(maSoiDataBaseSetting);
            _roomService = new RoomService(maSoiDataBaseSetting);
            _roomDetailService = new RoomDetailService(maSoiDataBaseSetting);
            _HostEnvironment = hostingEnvironment;
        }

        //[HttpGet]
        //public async Task<List<User>>  Get() =>
        //    await _userService.GetAsync();

        [HttpGet("{id:length(24)}")]
        [Route("SignIn_Get")]
        public async Task<ActionResult<User>> SignIn(string Tk, string Pass)
        {
            //Hash mật khẩu
            Pass = Other.MD5.CreateMD5(Pass);

            var user = await _userService.GetTk(Tk, Pass);

            if (user is null)
            {
                return Ok(new Response.Message(0, "Tài khoản hoặc mật khẩu bị sai", null));
            }

            return Ok(new Response.Message(1, "Đăng nhập thành công", null));
        }

        [HttpGet("{id:length(24)}")]
        [Route("GetInfo_Get")]
        public IActionResult GetInfo(string Tk)
        {
            try
            {
                var user = _userService.CheckTk(Tk);

                if (user is null)
                {
                    return Ok(new Response.Message_GetInfo(0, "Lấy dữ liệu thất bại", null));
                }

                return Ok(new Response.Message_GetInfo(1, "Lấy dữ liệu thành công", user));
            }
            catch(Exception e)
            {
                return Ok(new Response.Message_GetInfo(2, e.ToString(), null));
            }
        }

        [HttpPost]
        [Route("Registration_Post")]
        public async Task<IActionResult> Registration(User newUser)
        {
            //kiểm tra tìa khoản đã tồn tại
            var user = _userService.CheckTk(newUser.Tk);
            if(user != null)
            {
                return Ok(new Response.Message(0, "Tài khoản đã tồn tại", null));
            }

            //Hash mật khẩu
            newUser.Pass = Other.MD5.CreateMD5(newUser.Pass);
            newUser.Img = "";
            newUser.ImgBack = "";

            await _userService.CreateAsync(newUser);

            return Ok(new Response.Message(1, "Tạo tài khoản thành công", null));
        }

        [HttpPost]
        [Route("ChangePass_Post")]
        public async Task<IActionResult> ChangePass(Request.ChangePass_Request input)
        {
            //Hash mật khẩu
            input.PassOld = Other.MD5.CreateMD5(input.PassOld);

            var user = await _userService.GetTk(input.Tk, input.PassOld);

            if (user is null)
            {
                return Ok(new Response.Message(0, "Mật khẩu củ bị sai", null));
            }

            //Hash mật khẩu
            user.Pass = Other.MD5.CreateMD5(input.PassNew);

            await _userService.UpdateAsync(user);

            return Ok(new Response.Message(1, "Thay đổi mật khẩu thành công", null));
        }

        [HttpPost]
        [Route("UpdateSetting_Post")]
        public async Task<IActionResult> UpdateSetting(Request.Setting_Request input)
        {
            var user = _userService.CheckTk(input.Tk);

            if (user is null)
            {
                return NotFound();
            }

            user.Background = input.Background;
            user.Language = input.Language;

            await _userService.UpdateAsync(user);

            return Ok(new Response.Message(1, "Cài đặt thành công", null));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            //var user = await _userService.GetAsync(id);

            //if (user is null)
            //{
            //    return NotFound();
            //}

            //await _userService.RemoveAsync(user.Id);

            return NoContent();
        }

        [Route("UpdateImg_Post")]
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public async Task<IActionResult> UpdateImg(string Tk, string domain)
        {
            var user = _userService.CheckTk(Tk);
            string contentRootPath = _HostEnvironment.ContentRootPath;

            try
            {
                var photo = HttpContext.Request.Form.Files;
                foreach (var item in photo)
                {
                    string[] name = item.FileName.Split(".");
                    string imgName = "Picture/" + name[0] + "_" + Tk + "." + name[1];
                    var imaageSavePath = Path.Combine(contentRootPath, "Picture", name[0] + "_" + Tk + "." + name[1]);
                    if (!System.IO.File.Exists(imaageSavePath))
                    {
                        user.Img = domain+imgName;
                        await _userService.UpdateAsync(user);
                        var stream = System.IO.File.Create(imaageSavePath);
                        item.CopyToAsync(stream);
                    }
                }
            }
            catch
            {
                return Ok(new Response.Message(0, "Thêm ảnh thất bại", null));
            }
            return Ok(new Response.Message(1, "Thêm ảnh thành công", null));
        }

        [Route("UpdateImgBack_Post")]
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public async Task<IActionResult> UpdateImgBack(string Tk, string domain)
        {
            var user = _userService.CheckTk(Tk);
            string contentRootPath = _HostEnvironment.ContentRootPath;

            try
            {
                var photo = HttpContext.Request.Form.Files;
                foreach (var item in photo)
                {
                    string[] name = item.FileName.Split(".");
                    string imgName = "Picture/" + name[0] + "_" + Tk + "_BackGround." + name[1];
                    var imaageSavePath = Path.Combine(contentRootPath, "Picture", name[0] + "_" + Tk + "_BackGround." + name[1]);
                    if (!System.IO.File.Exists(imaageSavePath))
                    {
                        user.ImgBack = domain + imgName;
                        await _userService.UpdateAsync(user);
                        var stream = System.IO.File.Create(imaageSavePath);
                        item.CopyToAsync(stream);
                    }
                }
            }
            catch
            {
                return Ok(new Response.Message(0, "Thêm ảnh thất bại", null));
            }
            return Ok(new Response.Message(1, "Thêm ảnh thành công", null));
        }

        [Route("UpdateInfo_Post")]
        [HttpPost]
        public async Task<IActionResult> UpdateInfo(string Tk, string Name)
        {
            var user = _userService.CheckTk(Tk);

            user.FullName = Name;

            await _userService.UpdateAsync(user);

            return Ok(new Response.Message(1, "Cập nhật dữ liệu thành công", null));
        }

        [Route("CheckRoom_Get")]
        [HttpGet]
        public async Task<IActionResult> GetRoom(string Tk)
        {
            var check = await _roomDetailService.CheckUser(Tk);

            if(check is null)
            {
                return Ok(new Response.Message_GetRoom(0, "Không có trong phòng", null, null));
            }

            var room = await _roomService.GetAsync1(check.RoomId);

            if (room is null)
            {
                return NotFound();
            }

            var playerList = _roomDetailService.GetAllPlayer(check.RoomId);

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
                player.BaiId = item.BaiId;

                playerL.Add(player);
            }

            return Ok(new Response.Message_GetRoom(1, "Lấy dữ liệu thành công", room, playerL));
        }
    }
}
