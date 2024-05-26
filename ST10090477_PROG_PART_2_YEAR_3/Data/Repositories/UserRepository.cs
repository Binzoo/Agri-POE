using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ST10090477_PROG_PART_2_YEAR_3.Data.Interfaces;
using ST10090477_PROG_PART_2_YEAR_3.Models;
using ST10090477_PROG_PART_2_YEAR_3.Utlities;
using ST10090477_PROG_PART_2_YEAR_3.ViewModels;

namespace ST10090477_PROG_PART_2_YEAR_3.Data.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
          

        }

        public async Task<(bool success, string message)> LoginAsync(string email, string password, bool rememberMe)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(e => e.Email == email);
            if (user == null)
            {
                return (false, "Username does not exist");
            }
            var isPasswrodValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswrodValid)
            {
                return (false, "Password did not match");
            }
            await _signInManager.SignInAsync(user, rememberMe);
            return (true, "Login Successfully.");
        }

        public string LogOut()
        {
            _signInManager.SignOutAsync();
            return "LogOut Successfully.";
        }
        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<(bool success, string message, RegisterVM farmerviewmodel)> CreateUserAsync(RegisterVM registerVM)
        {
            var checkUserByEmail = await _userManager.FindByEmailAsync(registerVM.Email);
            if(checkUserByEmail != null) 
            {
                return (false, "Email alreday exists.", registerVM);
            }

            var applicatoinUser = new ApplicationUser()
            {
                Email = registerVM.Email,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                UserName = registerVM.Email
            };

            var result = await _userManager.CreateAsync(applicatoinUser, registerVM.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(applicatoinUser, registerVM.Role);
            }

            if(registerVM.Role == "Employee")
            {
                return (true, "Employee registered successfully.", registerVM);
            }

            return (true, "Farmer registered successfully.", registerVM);

        }

        public async Task<List<UserVM>> GetAllUserWithRoleAsync(string userRoleName)
        {
            var users =  await _userManager.Users.ToListAsync();

            var vm = users.Select(x => new UserVM()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,

            }).ToList();
            foreach (var value in vm)
            {
                var singleUser = await _userManager.FindByIdAsync(value.Id);
                var role = await _userManager.GetRolesAsync(singleUser);
                value.Role = role.FirstOrDefault();
            }

            if(userRoleName == WebsiteRoles.WebsiteEmployee)
            {
                var allEmployee = vm.Where(e => e.Role == WebsiteRoles.WebsiteEmployee).ToList();
                return allEmployee;
            }
            else if(userRoleName == WebsiteRoles.WebsiteFarmer)
            {
                var allEmployee = vm.Where(e => e.Role == WebsiteRoles.WebsiteFarmer).ToList();
                return allEmployee;
            }
            else
            {
                return vm;
            }

        }

        public async Task<ApplicationUser> FindUserById(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);
            return existingUser;
        }

        public async Task<(bool success, string message)> ResetPasswordAsync(ResetPasswrodVM vm)
        {
            var user = await _userManager.FindByIdAsync(vm.Id);
            if (user == null)
            {
                return (false, "User did not exist.");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token,vm.NewPassword);
            if (result.Succeeded)
            {
                return (true, "Password Reset Successfully.");
            }
            return (false, "Password Reset Unsuccessfull");
        }
    }
}
