

using System.ComponentModel.DataAnnotations;

namespace WebApi.MinimalApi.Models;

public record CreateUserModel
{
    [Required]
    public string Login { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
}