using System.ComponentModel.DataAnnotations;

namespace AcxiomCRM.Models;

public class Activity
{
    public int ActivityId { get; set; }

    [Required]
    [StringLength(50)]
    public string ActivityType { get; set; } = "Call";

    [Required]
    [StringLength(150)]
    public string Subject { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime ActivityDate { get; set; } = DateTime.Today;

    public int? CustomerId { get; set; }

    public int? LeadId { get; set; }

    public string? AssignedTo { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Planned";
}