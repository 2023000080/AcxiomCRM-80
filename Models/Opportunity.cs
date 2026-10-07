using System.ComponentModel.DataAnnotations;

namespace AcxiomCRM.Models;

public class Opportunity
{
    public int OpportunityId { get; set; }

    [Required]
    [StringLength(150)]
    public string OpportunityName { get; set; } = string.Empty;

    [Required]
    public int CustomerId { get; set; }

    public int? LeadId { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    [Required]
    [StringLength(50)]
    public string Stage { get; set; } = "Qualification";

    [Range(0, 100)]
    public int Probability { get; set; }

    [DataType(DataType.Date)]
    public DateTime ExpectedCloseDate { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "Active";

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public string? AssignedTo { get; set; }

    [StringLength(100)]
    public string? Source { get; set; }

    public string? Notes { get; set; }
}