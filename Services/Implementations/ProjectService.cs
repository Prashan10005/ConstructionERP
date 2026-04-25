using ConstructionERP.Data;
using ConstructionERP.DTOs.Project;
using ConstructionERP.Models.Entities;
using ConstructionERP.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConstructionERP.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto, string currentUser)
        {
            var project = new Project
            {
                ProjectName = dto.ProjectName,
                Description = dto.Description,
                Status = "Open",
                ProjectManager = currentUser,
                StartDate = DateTime.Now.Date,
                EndDate = null
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return MapToDto(project);
        }
        
        public async Task<List<ProjectDto>> GetAllAsync()
        {
            return await _context.Projects
                .Select(p => new ProjectDto
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    Status = p.Status,
                    ProjectManager = p.ProjectManager,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate
                })
                .ToListAsync();
        }
        public async Task<ProjectStatusDto> GetProjectStatusByIdAsync(int projectId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);

            if (project == null)
                return null;

            return new ProjectStatusDto
            {
                ProjectId = project.ProjectId,
                Status = project.Status
            };
        }

        public async Task<bool> UpdateProjectStatusAsync(UpdateProjectStatusDto dto)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == dto.ProjectId);

            if (project == null)
                return false;

            // rule for preventing to change the project status if task is pending
            var hasOpenTask = await _context.ProjectTasks
                .AnyAsync(t => t.ProjectID == dto.ProjectId && t.Status == "Open");

            if (hasOpenTask)
            {
                throw new Exception("Cannot update project status. There are open tasks under this project.");
            }

            // business rule: prevent reopening closed projects
            if (project.Status == "Closed")
                throw new Exception("Closed project cannot be modified");

            // allowed transitions only
            if (dto.Status != "Closed" && dto.Status != "Cancelled")
                throw new Exception("Invalid status update");

            project.Status = dto.Status;

            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return true;
        }

        private ProjectDto MapToDto(Project project)
        {
            return new ProjectDto
            {
                ProjectId = project.ProjectId,
                ProjectName = project.ProjectName,
                Description = project.Description,
                Status = project.Status,
                ProjectManager = project.ProjectManager,
                StartDate = project.StartDate,
                EndDate = project.EndDate
            };
        }
    }
}
