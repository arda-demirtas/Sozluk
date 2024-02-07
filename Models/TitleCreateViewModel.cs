using System.ComponentModel.DataAnnotations;

namespace Sozluk.Models
{
    public class TitleCreateViewModel
    {
        [Required]
        [Display(Name = "Title")]
        public string? Text { get; set; }
    }
}
