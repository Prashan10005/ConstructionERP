using System.ComponentModel.DataAnnotations;

namespace ConstructionERP.Models.Entities
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public string Description { get; set; }

        public string Status { get; set; } // Open, InProgress, Closed, Cancelled
        
        public string ProjectManager { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
