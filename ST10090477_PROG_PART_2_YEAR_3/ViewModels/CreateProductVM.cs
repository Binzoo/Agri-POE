using ST10090477_PROG_PART_2_YEAR_3.Models;
using System.ComponentModel.DataAnnotations;

namespace ST10090477_PROG_PART_2_YEAR_3.ViewModels
{
    public class CreateProductVM
    {
        public int ProductId { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public string? ProductCategory { get; set; }
        [Required]
        public DateTime? ProductProductionDate { get; set; }
        public string? ProductImageUrl { get; set; }
        public IFormFile? Thumbnail { get; set; }
        public string? ApplicationUserId { get; set; }
    }
}
