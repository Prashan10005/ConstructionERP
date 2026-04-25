namespace ConstructionERP.DTOs.ProjectTask
{
    public class TaskResponseDto
    {
        public int TaskId { get; set; }

        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public int ProjectId { get; set; }

        public int AssignedToId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; }
    }
}
