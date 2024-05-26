using Microsoft.AspNetCore.Mvc;
using ST10090477_PROG_PART_2_YEAR_3.Models;
using System.Diagnostics;

namespace ST10090477_PROG_PART_2_YEAR_3.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            return View();  
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
