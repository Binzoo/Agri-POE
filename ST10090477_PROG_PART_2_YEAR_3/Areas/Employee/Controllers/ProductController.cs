using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using NuGet.Packaging.Signing;
using ST10090477_PROG_PART_2_YEAR_3.Data.Interfaces;
using ST10090477_PROG_PART_2_YEAR_3.Utlities;
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

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            if (User.IsInRole(WebsiteRoles.WebsiteEmployee))
            {
                var allProducts = await _product.GetAllProduct();
                if (allProducts == null)
                {
                    _notyfService.Success("There is no Product.");
                    return View(allProducts);
                }
                else
                {

                    if (startDate.HasValue)
                    {
                        allProducts = allProducts.Where(p => p.ProductProductionDate >= startDate.Value).ToList();
                    }

                    if (endDate.HasValue)
                    {
                        allProducts = allProducts.Where(p => p.ProductProductionDate <= endDate.Value).ToList();
                    }

                    ViewData["startDate"] = startDate?.ToString("yyyy-MM-dd");
                    ViewData["endDate"] = endDate?.ToString("yyyy-MM-dd");

                    _notyfService.Success("Products of all farmers.");
                    return View(allProducts);
                }
            }
            else
            {
                var products = await _product.GetAllProductByIDAsync(User);
                return View(products);
            }
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
            if (success)
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
            var products = await _product.ViewSpecificUserProductAsync(id);

            if (products == null)
            {
                _notyfService.Success("No products to show.");
            }
            _notyfService.Success("All Products.");
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            int num = int.Parse(id);
            var product = await _product.GetProductById(num);
            if (product != null)
            {
                return View(product);
            }
            _notyfService.Error("Product dose not exist.");
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreateProductVM vm)
        {
            if (ModelState.IsValid)
            {
                var (success, message) = await _product.EditProduct(vm);

                if (success)
                {
                    _notyfService.Success(message);
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    _notyfService.Error(message);
                    return RedirectToAction("Index", "Product");
                }
            }
            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            int num = int.Parse(id);
            var product = await _product.GetProductById(num);
            if (product != null)
            {
                return View(product);
            }
            _notyfService.Error("Product dose not exist.");
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int productID)
        {
            if (productID < 0)
            {
                _notyfService.Error("Product is not found.");
                return RedirectToAction("Index", "Product");
            }
            else
            {
                var (success, message) = await _product.DeleteProduct(productID);
                if (success)
                {
                    _notyfService.Success(message);
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    _notyfService.Error(message);
                    return RedirectToAction("Index", "Product");
                }
            }
        }

    }
}
