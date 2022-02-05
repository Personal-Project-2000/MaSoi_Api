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
    public class StoryController : Controller
    {
        private readonly StoryService _storyService;
        public StoryController(IOptions<MaSoiDatabaseSettings> maSoiDataBaseSetting)
        {
            _storyService = new StoryService(maSoiDataBaseSetting);
        }

        [HttpPost]
        [Route("CreateStory_Post")]
        public async Task<IActionResult> AddStory(Story story)
        {
            await _storyService.CreateAsync(story);

            return Ok(new Response.Message(1, "Thêm dữ liệu thành công", null));
        }
    }
}
