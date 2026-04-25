using ConstructionERP.DTOs.Material;
using ConstructionERP.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionERP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MaterialController : ControllerBase
    {

        private readonly IMaterialService _materialService;

        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [Authorize(Roles = "ProjectManager")]
        [HttpGet("projects")]
        public async Task<IActionResult> GetProjects()
        {
            var data = await _materialService.GetProgressProjectsAsync();
            return Ok(data);
        }

        [HttpGet("tasks")]
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> GetProjectTasks(int projectId)
        {
            var tasks = await _materialService.GetProjectTasksAsync(projectId);
            return Ok(tasks);
        }

        [HttpPost("request")]
        [Authorize(Roles = "ProjectManager")]
        public async Task<IActionResult> CreateMaterialRequest([FromBody] CreateMaterailRequestDto dto)
        {
            try
            {
                var username = User.Identity.Name;

                var result = await _materialService.CreateMaterialRequestAsync(dto, username);

                return Ok(new { success = true });

            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}