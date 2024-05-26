using ST10090477_PROG_PART_2_YEAR_3.Models;
using ST10090477_PROG_PART_2_YEAR_3.ViewModels;

namespace ST10090477_PROG_PART_2_YEAR_3.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<(bool success, string message)> LoginAsync(string email, string password, bool rememberMe);
        string LogOut();
        Task<List<ApplicationUser>> GetAllUsersAsync();

        Task<(bool success, string message, RegisterVM farmerviewmodel)> CreateUserAsync(RegisterVM createFarmerVM);

        Task<List<UserVM>> GetAllUserWithRoleAsync(string userRoleName);

        Task<(bool success, string message)> ResetPasswordAsync(ResetPasswrodVM vm);
        Task<ApplicationUser> FindUserById(string id);
    }
}
