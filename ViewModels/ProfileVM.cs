using System.ComponentModel.DataAnnotations;

namespace TokoSaya.ViewModels
{
    public class ProfileVM
    {
        [Required(ErrorMessage = "Nama tidak boleh kosong")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email tidak boleh kosong")]
        [EmailAddress(ErrorMessage = "Format email tidak valid")]
        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
    }
}