using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ST10090477_PROG_PART_2_YEAR_3.Data.Interfaces;
using ST10090477_PROG_PART_2_YEAR_3.Models;
using ST10090477_PROG_PART_2_YEAR_3.Utlities;

namespace ST10090477_PROG_PART_2_YEAR_3.Data.Repositories
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = applicationDbContext; 
            _userManager = userManager;
            _roleManager = roleManager;
        }

     
        public async Task Initalize()
        {
            if (!await _roleManager.RoleExistsAsync(WebsiteRoles.WebsiteEmployee))
            {
                await _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteEmployee));
                await _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteFarmer));
                await _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                    FirstName = "Super",
                    LastName = "Admin"
                }, "Admin@123");
            }

            var appUser = await _context.ApplicationUsers.FirstOrDefaultAsync(e => e.Email == "admin@gmail.com");
            if (appUser != null)
            {
                await _userManager.AddToRoleAsync(appUser, WebsiteRoles.WebsiteEmployee);
            }

            await _context.SaveChangesAsync();
        }

    }
}
