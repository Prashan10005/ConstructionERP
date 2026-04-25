using ConstructionERP.Data;
using ConstructionERP.DTOs.ProjectTask;
using ConstructionERP.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ConstructionERP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _service;
        private readonly ApplicationDbContext _context;

        public TaskController(ITaskService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        [Authorize(Roles = "ProjectManager")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDto dto)
        {
            try
            {
                var currentUser = User.Identity.Name;
                var result = await _service.CreateTaskAsync(dto, currentUser);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [Authorize(Roles = "ProjectManager")]
        [HttpGet("projects")]
        public async Task<IActionResult> GetProjects()
        {
            var data = await _service.GetActiveProjectsAsync();
            return Ok(data);
        }

        [Authorize(Roles = "ProjectManager")]
        [HttpGet("fieldstaff")]
        public async Task<IActionResult> GetFieldStaff()
        {
            var data = await _service.GetFieldStaffAsync();
            return Ok(data);
        }

        [Authorize(Roles = "FieldStaff")]
        [HttpGet("mytasks")]
        public async Task<IActionResult> GetMyTasks()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var tasks = await _service.GetMyTasksAsync(userId);

            return Ok(tasks);
        }

        [Authorize(Roles = "FieldStaff")]
        [HttpPut("complete")]
        public async Task<IActionResult> CompleteTask([FromBody] CompleteTaskDto dto)
        {
            try
            {
                
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var result = await _service.CompleteTaskAsync(dto.TaskId, userId);

                if (!result)
                    return BadRequest(new { success = false, message = "Task not found" });

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
