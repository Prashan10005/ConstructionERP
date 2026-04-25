using ConstructionERP.DTOs.Material;
using ConstructionERP.DTOs.Project;
using ConstructionERP.DTOs.ProjectTask;

namespace ConstructionERP.Services.Interfaces
{
    public interface IMaterialService
    {
        Task<bool> CreateMaterialRequestAsync(CreateMaterailRequestDto dto, string username);

        Task<List<ProjectDropdownDto>> GetProgressProjectsAsync();

        Task<List<TaskDropdownDto>> GetProjectTasksAsync(int projectId);
    }
}
