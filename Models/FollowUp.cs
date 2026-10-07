using System.ComponentModel.DataAnnotations;

namespace AcxiomCRM.Models;

public class FollowUp
{
    public int FollowUpId { get; set; }

    public int? CustomerId { get; set; }

    public int? LeadId { get; set; }

    public int? OpportunityId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime FollowUpDate { get; set; } = DateTime.Today;

    [Required]
    [StringLength(50)]
    public string FollowUpType { get; set; } = "Call";

    [StringLength(500)]
    public string? Remarks { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Planned";

    public string? AssignedTo { get; set; }
}