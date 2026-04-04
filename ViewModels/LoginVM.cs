using System.ComponentModel.DataAnnotations;

namespace TokoSaya.ViewModels;

public class LoginVM
{
    [Required]
    public string Email {get; set;} = null!;
    [Required]
    public string Password {get; set;} = null!;
    public bool RememberMe {get; set;}
}