using ConstructionERP.DTOs.Project;
using ConstructionERP.DTOs.ProjectTask;

namespace ConstructionERP.Services.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto, string currentUser);
        Task<List<ProjectDto>> GetAllAsync();
        Task<ProjectStatusDto> GetProjectStatusByIdAsync(int projectId);
        Task<bool> UpdateProjectStatusAsync(UpdateProjectStatusDto dto);

    }
}
