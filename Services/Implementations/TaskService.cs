namespace ConstructionERP.Services.Implementations
{
    using ConstructionERP.Data;
    using ConstructionERP.DTOs.Project;
    using ConstructionERP.DTOs.ProjectTask;
    using ConstructionERP.DTOs.User;
    using ConstructionERP.Models.Entities;
    using ConstructionERP.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto dto, string currentUser)
        {
            var project = await _context.Projects
                 .FirstOrDefaultAsync(p => p.ProjectId == dto.ProjectId);
            var task = new ProjectTask
            {
                TaskName = dto.TaskName,
                TaskDescription = dto.TaskDescription,
                ProjectID = dto.ProjectId,
                AssignedToId = dto.AssignedToId,
                CreatedBy = currentUser,
                CreatedAt = DateTime.Now,
                Status = "Open"
            };

            _context.ProjectTasks.Add(task);

           if (project.Status != "InProgress")
            {
                project.Status = "InProgress";
                _context.Projects.Update(project);
            }

            await _context.SaveChangesAsync();

            return MapTaskTo(task);
        }

        public async Task<List<ProjectDropdownDto>> GetActiveProjectsAsync()
        {
            return await _context.Projects
                .Where(p => p.Status == "Open" || p.Status == "InProgress")
                .Select(p => new ProjectDropdownDto
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName
                })
                .ToListAsync();
        }

        public async Task<List<UserDropdownDto>> GetFieldStaffAsync()
        {
            return await _context.Users
                .Where(u => u.Role == "FieldStaff" && u.IsActive)
                .Select(u => new UserDropdownDto
                {
                    UserId = u.UserID,
                    Username = u.Username
                })
                .ToListAsync();
        }

        private TaskResponseDto MapTaskTo(ProjectTask t)
        {
            return new TaskResponseDto
            {
                TaskId = t.TaskId,
                TaskName = t.TaskName,
                TaskDescription = t.TaskDescription,
                ProjectId = t.ProjectID,
                AssignedToId = t.AssignedToId,
                CreatedBy = t.CreatedBy,
                CreatedAt = t.CreatedAt,
                Status = t.Status
            };
        }

        public async Task<List<FieldStaffTaskDto>> GetMyTasksAsync(int userId)
        {
            return await _context.ProjectTasks
                .Where(t => t.AssignedToId == userId && t.Status == "Open")
                .Join(_context.Projects,
                    t => t.ProjectID,
                    p => p.ProjectId,
                    (t, p) => new FieldStaffTaskDto
                    {
                        TaskId = t.TaskId,
                        TaskName = t.TaskName,
                        TaskDescription = t.TaskDescription,
                        ProjectName = p.ProjectName,
                        Status = t.Status
                    })
                .ToListAsync();
        }

        public async Task<bool> CompleteTaskAsync(int taskId, int userId)
        {
            var task = await _context.ProjectTasks
                .FirstOrDefaultAsync(t => t.TaskId == taskId);

            if (task == null)
                return false;

            if (task.Status == "Completed")
                throw new Exception("Task already completed");

            task.Status = "Completed";
            task.CompletedAt = DateTime.Now;
            task.CompletedById = userId;

            _context.ProjectTasks.Update(task);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
