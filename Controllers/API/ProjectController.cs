using ConstructionERP.DTOs.Project;
using ConstructionERP.DTOs.ProjectTask;
using ConstructionERP.Services.Implementations;
using ConstructionERP.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConstructionERP.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ProjectManager")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectController(IProjectService service)
        {
            _service = service;
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectDto dto)
        {
            try
            {
                string currentUser = User.Identity.Name;
                var result = await _service.CreateProjectAsync(dto, currentUser);

                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        
        // load project grid
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _service.GetAllAsync();
            return Ok(new { success = true, data = projects });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var result = await _service.GetProjectStatusByIdAsync(id);

            if (result == null)
                return NotFound(new { success = false, message = "Project not found" });

            return Ok(new { success = true, data = result });
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateProjectStatusDto dto)
        {
            try
            {
                var result = await _service.UpdateProjectStatusAsync(dto);
            if (!result)
                return BadRequest(new { success = false, message = "Project not found" });

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
