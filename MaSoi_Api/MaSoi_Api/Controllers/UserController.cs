using MaSoi_Api.Models;
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
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService) =>
            _userService = userService;

        [HttpGet]
        public async Task<List<User>>  Get() =>
            await _userService.GetAsync();

        [HttpGet("{id:length(24)}")]
        [Route("SignIn")]
        public async Task<ActionResult<User>> Get(string Tk, string Pass)
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

        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration(User newUser)
        {
            //kiểm tra tìa khoản đã tồn tại
            var user = await _userService.CheckTk(newUser.Tk);
            if(user != null)
            {
                return Ok(new Response.Message(0, "Tài khoản đã tồn tại", null));
            }

            //Hash mật khẩu
            newUser.Pass = Other.MD5.CreateMD5(newUser.Pass);

            await _userService.CreateAsync(newUser);

            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, User updatedUser)
        {
            //var user = await _userService.GetAsync(id);

            //if (user is null)
            //{
            //    return NotFound();
            //}

            //updatedUser.Id = user.Id;

            //await _userService.UpdateAsync(id, updatedUser);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
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
    }
}
