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


        public void Initialize()
        {
            if (!_roleManager.RoleExistsAsync(WebsiteRoles.WebsiteEmployee).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteEmployee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebsiteRoles.WebsiteFarmer)).GetAwaiter().GetResult();
                _userManager.CreateAsync(new ApplicationUser()
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    FirstName = "Super",
                    LastName = "Admin"
                }, "Admin@0011").Wait();

                var appUser = _context.ApplicationUsers!.FirstOrDefault(x => x.Email == "admin@gmail.com");
                if (appUser != null)
                {
                    _userManager.AddToRoleAsync(appUser, WebsiteRoles.WebsiteEmployee).GetAwaiter().GetResult();
                }
                _context.SaveChanges();
            }
        }
    }
}