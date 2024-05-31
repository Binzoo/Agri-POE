using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using ST10090477_PROG_PART_2_YEAR_3.Data.Interfaces;
using ST10090477_PROG_PART_2_YEAR_3.Models;
using ST10090477_PROG_PART_2_YEAR_3.Utlities;
using ST10090477_PROG_PART_2_YEAR_3.ViewModels;

namespace ST10090477_PROG_PART_2_YEAR_3.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly INotyfService _notyfService;

        public UserController(INotyfService notyfService, IUserRepository userRepository)
        {
            _notyfService = notyfService;
            _userRepository = userRepository;
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUserWithRoleAsync("All");
            return View(users);
        }


        [Authorize(Roles = "Employee")]
        [HttpGet]
        public async Task<IActionResult> IndexFarmer()
        {
            var users = await _userRepository.GetAllUserWithRoleAsync("Farmer"); ;

            return View(users);
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public async Task<IActionResult> IndexEmployee()
        {
            var users = await _userRepository.GetAllUserWithRoleAsync("Employee"); ;

            return View(users);
        }


        [Authorize(Roles = "Employee")]
        [HttpGet]
        public IActionResult CreateFarmer()
        {

            return View(new RegisterVM());
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> CreateFarmer(RegisterVM vm)
        {
            vm.Role = WebsiteRoles.WebsiteFarmer;
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var (success, message, farmerviewmodel) = await _userRepository.CreateUserAsync(vm);
            if (!success)
            {
                _notyfService.Error(message);
                return View(farmerviewmodel);
            }
            else
            {
                _notyfService.Success(message);
                return RedirectToAction("Index", "User", new { area = "Employee" });
            }
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public IActionResult CreateEmployee()
        {

            return View(new RegisterVM());
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> CreateEmployee(RegisterVM vm)
        {
            vm.Role = WebsiteRoles.WebsiteEmployee;
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var (success, message, farmerviewmodel) = await _userRepository.CreateUserAsync(vm);
            if (!success)
            {
                _notyfService.Error(message);
                return View(farmerviewmodel);
            }
            else
            {
                _notyfService.Success(message);
                return RedirectToAction("Index", "User", new { area = "Employee" });
            }
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity!.IsAuthenticated)
            {
                return View(new LoginVM { });
            }
            return RedirectToAction("Index", "Product", new { area = "Employee" });
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var (success, message) = await _userRepository.LoginAsync(vm.Email, vm.Password, vm.RememberMe);
            if (!success)
            {
                _notyfService.Error(message);
                return View(vm);
            }

            _notyfService.Success(message);
            return RedirectToAction("Index", "Product", new { area = "Employee" });
        }

        [HttpPost]
        [Authorize]
        public IActionResult LogOut()
        {
            _userRepository.LogOut();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [Authorize(Roles = "Employee")]
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var userExist = await _userRepository.FindUserById(id);
            if (userExist == null)
            {
                _notyfService.Error("User does not exist");
                return View();
            }

            var resetPasswordVm = new ResetPasswrodVM()
            {
                Id = userExist.Id,
                Email = userExist.Email,
            };

            return View(resetPasswordVm);
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswrodVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var (success, message) = await _userRepository.ResetPasswordAsync(vm);

            if (success)
            {
                _notyfService.Success(message);
                return RedirectToAction("Index", "User", new { area = "Employee" });
            }
            else
            {
                _notyfService.Error(message);
                return View(vm);
            }
        }

        [HttpGet("AccessDenied")]
        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
