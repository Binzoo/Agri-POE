using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ST10090477_PROG_PART_2_YEAR_3.Models;
using ST10090477_PROG_PART_2_YEAR_3.ViewModels;

namespace ST10090477_PROG_PART_2_YEAR_3.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class UserController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;  
        private readonly INotyfService _notyfService;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, INotyfService notyfService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _notyfService = notyfService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            if(!HttpContext.User.Identity.IsAuthenticated)
            {
                return View(new LoginVM { });
            }
            return RedirectToAction("Index", "User", new { area = "Employee" });
        }

        [HttpPost("Login")]     
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(e => e.Email == vm.Email);
            if(existingUser == null)
            {
                _notyfService.Error("Username does not exist.");
                return View(vm);
            }

            var verifyPassword = await _userManager.CheckPasswordAsync(existingUser, vm.Password);
            if(!verifyPassword)
            {
                _notyfService.Error("Password did not match.");
                return View(vm);
            }

            await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, vm.RememberMe, true);
            _notyfService.Success("Login Successfull.");

            return RedirectToAction("Index", "User", new { area = "Employee" });
        }
    }
}
