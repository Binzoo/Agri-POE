using ST10090477_PROG_PART_2_YEAR_3.Utlities;
using System.ComponentModel.DataAnnotations;

namespace ST10090477_PROG_PART_2_YEAR_3.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
