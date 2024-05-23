using System.ComponentModel.DataAnnotations;

namespace ST10090477_PROG_PART_2_YEAR_3.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
