using System.ComponentModel.DataAnnotations;

namespace Core;

public class Developer
{
    [Key] 
    public Guid DeveloperId { get; } = Guid.NewGuid();
    [MaxLength(255)]
    public string DeveloperName { get; set; }
}