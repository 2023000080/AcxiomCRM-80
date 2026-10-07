using System.ComponentModel.DataAnnotations;

namespace AcxiomCRM.Models;

public class Lead
{
    public int LeadId { get; set; }

    [Required]
    [StringLength(50)]
    public string LeadCode { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    public string LeadName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [StringLength(150)]
    public string? CompanyName { get; set; }

    [StringLength(100)]
    public string? Source { get; set; }

    [Required]
    [StringLength(50)]
    public string Status { get; set; } = "New";

    [Range(0, double.MaxValue)]
    public decimal ExpectedValue { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public string? AssignedTo { get; set; }
}