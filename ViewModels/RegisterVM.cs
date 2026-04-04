using System.ComponentModel.DataAnnotations;

namespace TokoSaya.ViewModels;

public class RegisterVM
{
    [Required]
    public string Name {get; set;} = null!;
    [Required]
    public string Email {get; set;} = null!;
    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password {get; set;} = null!;

    [Required]
    [Compare("Password")]
    public string ConfirmPassword {get; set;} = null!;

}