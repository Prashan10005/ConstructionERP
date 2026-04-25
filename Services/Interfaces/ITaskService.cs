using ConstructionERP.DTOs.Project;
using ConstructionERP.DTOs.ProjectTask;
using ConstructionERP.DTOs.User;

namespace ConstructionERP.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto dto, string currentUser);
        Task<List<ProjectDropdownDto>> GetActiveProjectsAsync();

        Task<List<UserDropdownDto>> GetFieldStaffAsync();

        Task<List<FieldStaffTaskDto>> GetMyTasksAsync(int userId);

        Task<bool> CompleteTaskAsync(int taskId, int userId);
    }
}
