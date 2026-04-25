using ConstructionERP.Data;
using ConstructionERP.DTOs.Material;
using ConstructionERP.DTOs.Project;
using ConstructionERP.DTOs.ProjectTask;
using ConstructionERP.Models.Entities;
using ConstructionERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConstructionERP.Services.Implementations
{
    public class MaterialService : IMaterialService
    {
        private readonly ApplicationDbContext _context;

        public MaterialService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateMaterialRequestAsync(CreateMaterailRequestDto dto, string username)
        {
            var task = await _context.ProjectTasks.FirstOrDefaultAsync(t => t.TaskId == dto.TaskId);

            if (task == null)
                throw new Exception("Task not found");

            var request = new MaterialRequest
            {
                ProjectId = dto.ProjectId,
                TaskId = dto.TaskId,
                MaterialName = dto.MaterialName,

                AssignedTo = task.AssignedToId,
                RequestedBy = username,
                RequestedAt = DateTime.UtcNow,
                Status = "Pending"
            };

            _context.MaterialRequest.Add(request);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProjectDropdownDto>> GetProgressProjectsAsync()
        {
            return await _context.Projects
                .Where(p => p.Status == "InProgress")
                .Select(p => new ProjectDropdownDto
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName
                })
                .ToListAsync();
        }

        public async Task<List<TaskDropdownDto>> GetProjectTasksAsync(int projectId)
        {
            return await _context.ProjectTasks
                .Where(t => t.ProjectID == projectId && t.Status == "Open")
                .Select(t => new TaskDropdownDto
                {
                    TaskId = t.TaskId,
                    TaskName = t.TaskName
                }).ToListAsync();
        }
    }
}
