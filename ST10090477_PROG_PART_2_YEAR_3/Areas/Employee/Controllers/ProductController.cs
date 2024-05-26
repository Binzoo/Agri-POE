using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using ST10090477_PROG_PART_2_YEAR_3.Data.Interfaces;
using ST10090477_PROG_PART_2_YEAR_3.ViewModels;

namespace ST10090477_PROG_PART_2_YEAR_3.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize]
    public class ProductController : Controller
    {

        private readonly IProduct _product;
        private readonly INotyfService _notyfService;

        public ProductController(IProduct product, INotyfService notyfService)
        {
            _product = product;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _product.GetAllProductByIDAsync(User);
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateProductVM() { });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM createProductVM)
        {
            if (!ModelState.IsValid) { return View(createProductVM); }

            var (success, message) = await _product.CreateProduct(createProductVM, User);
            if(success)
            {
                _notyfService.Success("Product Created Successfully.");
                return RedirectToAction("Index");
            }
            else
            {
                _notyfService.Error("No Product Created.");
                return View(new CreateProductVM());
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewProductListing(string id)
        {
          var products =   await _product.ViewSpecificUserProductAsync(id);

            if(products == null)
            {
                _notyfService.Success("No products to show.");
            }
            _notyfService.Success("All Products.");
            return View(products);
        }
    }
}
