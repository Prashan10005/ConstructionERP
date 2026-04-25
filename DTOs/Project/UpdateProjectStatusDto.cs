namespace ConstructionERP.DTOs.Project
{
    public class UpdateProjectStatusDto
    {
        public int ProjectId { get; set; }
        public string Status { get; set; } // Closed / Cancelled
    }
}
