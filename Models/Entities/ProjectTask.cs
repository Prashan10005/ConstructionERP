using System.ComponentModel.DataAnnotations;

namespace ConstructionERP.Models.Entities
{
    public class ProjectTask
    {
        [Key]
        public int TaskId { get; set; }

        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public int ProjectID { get; set; }

        public int AssignedToId { get; set; } // FieldStaff username

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public int? CompletedById { get; set; }

        public string Status { get; set; } // Open, Completed
    }
}
