using Microsoft.AspNetCore.Identity;

namespace ST10090477_PROG_PART_2_YEAR_3.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<Product>? Products { get; set; }
    }
}
