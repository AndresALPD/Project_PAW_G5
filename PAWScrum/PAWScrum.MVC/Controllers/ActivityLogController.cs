using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAWScrum.Models.DTOs.ActivityLog;
using PAWScrum.Models.Entities;
using PAWScrum.Services.Interfaces;

namespace PAWScrum.MVC.Controllers
{
    [Route("[controller]")]
    public class ActivityLogController : Controller
    {
        private readonly IActivityLogService _service;
        public ActivityLogController(IActivityLogService service) => _service = service;

        private static IEnumerable<ActivityLogResponseDto> ToDto(IEnumerable<ActivityLog> src) =>
            src.Select(a => new ActivityLogResponseDto
            {
                ActivityId = a.ActivityId,
                UserId = (int)a.UserId,
                ProjectId = (int)a.ProjectId,
                Action = a.Action,
                Timestamp = a.Timestamp,
                UserName = a.User?.Username,
                ProjectName = a.Project?.ProjectName
            });

        [HttpGet("")]
        public IActionResult Index() =>
            View(Enumerable.Empty<ActivityLogResponseDto>());

        [HttpGet("project/{projectId:int}")]
        public async Task<IActionResult> ByProject(int projectId)
        {
            var data = await _service.GetByProjectAsync(projectId);
            ViewBag.ProjectId = projectId;
            return View("Index", ToDto(data));
        }

        [HttpGet("{projectId:int}/recent")]
        public async Task<IActionResult> Recent(int projectId, int take = 20)
        {
            var data = await _service.GetRecentAsync(projectId, take);
            ViewBag.ProjectId = projectId;
            return View("Recent", ToDto(data));
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> ByUser(int userId)
        {
            var data = await _service.GetByUserAsync(userId);
            ViewBag.UserId = userId;
            return View("Index", ToDto(data));
        }
    }
}