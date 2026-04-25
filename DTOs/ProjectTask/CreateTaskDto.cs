namespace ConstructionERP.DTOs.ProjectTask
{
    public class CreateTaskDto
    {
        public int ProjectId { get; set; }

        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public int AssignedToId { get; set; }
    }
}
