using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ST10090477_PROG_PART_2_YEAR_3.Data.Interfaces;
using ST10090477_PROG_PART_2_YEAR_3.Models;
using ST10090477_PROG_PART_2_YEAR_3.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace ST10090477_PROG_PART_2_YEAR_3.Data.Repositories
{
    public class ProductRepository : IProduct
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductRepository(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = applicationDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
        }
       
        public async Task<(bool success, string message)> CreateProduct(CreateProductVM createProductVM, ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = new Product()
            {
                ProductName = createProductVM.ProductName,
                ProductCategory = createProductVM.ProductCategory,
                ProductProductionDate = createProductVM.ProductProductionDate,
                ApplicationUserId = userId,
            };

            if (createProductVM.Thumbnail != null)
            {
                product.ProductImageUrl = UploadImage(createProductVM.Thumbnail);
            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return (true, "Prodcut Created Successfully.");
        }

        public async Task<List<CreateProductVM>> GetAllProductByIDAsync(ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId != null)
            {
                var allProductOfUser = await  _context.Products.Where(e => e.ApplicationUserId == userId).
                    Select(p => new CreateProductVM
                    {
                        ProductName = p.ProductName,
                        ProductCategory = p.ProductCategory,    
                        ProductProductionDate = p.ProductProductionDate, 
                        ProductImageUrl = p.ProductImageUrl
                        
                    }).ToListAsync();
                return allProductOfUser;
            }

            return new List<CreateProductVM>();
        }

        public async Task<List<CreateProductVM>> ViewSpecificUserProductAsync(string id)
        {
            var products = await _context.Products.Where(e => e.ApplicationUserId == id).Select(p => new CreateProductVM
            {
                ProductName = p.ProductName,
                ProductCategory = p.ProductCategory,
                ProductProductionDate = p.ProductProductionDate,
                ProductImageUrl = p.ProductImageUrl

            }).ToListAsync();
            return products;
        }

        private string UploadImage(IFormFile file)
        {
            string uniqueFileName = "";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Thumbnail");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
    }
}
