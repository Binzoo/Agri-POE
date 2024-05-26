using System.ComponentModel.DataAnnotations;

namespace ST10090477_PROG_PART_2_YEAR_3.ViewModels
{
    public class ResetPasswrodVM
    {

        public string Id { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? NewPassword  { get; set; }
        [Required]
        [Compare(nameof(NewPassword))]
        public string? ConfirmPassword { get; set; }
    }
}
