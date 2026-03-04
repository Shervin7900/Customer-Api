using System.ComponentModel.DataAnnotations;

namespace Customer_Api.Models;

public class Customer
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = default!;
    
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = default!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
