using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace ST10090477_PROG_PART_2_YEAR_3.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductProductionDate { get; set; }
        public string? ProductImage { get; set; }
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}