using System.ComponentModel.DataAnnotations;

namespace Sozluk.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Takma Ad")]
        public string? NickName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Eposta")]
        public string? Email { get; set; }
        [Required]
        [Display(Name = "Parola")]
        [StringLength(10, ErrorMessage = "{0} alanı en az {2} karakter uzunluğunda olmalı", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Parola tekrar")]
        [Compare(nameof(Password), ErrorMessage = "parola eslesmiyor.")]
        public string? ConfirmPassword { get; set; }
    }
}
