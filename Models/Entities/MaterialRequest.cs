using System.ComponentModel.DataAnnotations;

namespace ConstructionERP.Models.Entities
{
    public class MaterialRequest
    {
        [Key]
        public int MaterialRequestId { get; set; }  

        public int ProjectId { get; set; }

        public int TaskId { get; set; } 

        public string MaterialName { get; set; }

        public int AssignedTo { get; set; }

        public string RequestedBy { get; set; }

        public DateTime RequestedAt { get; set; }

        public int? Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public string Status { get; set; } // Pending, Approved, Rejected

        public DateTime? ApprovedDate { get; set; }
    }
}
