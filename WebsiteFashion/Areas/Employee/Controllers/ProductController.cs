using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebsiteFashion.Models;
using WebsiteFashion.Repositories;

namespace WebsiteFashion.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee")]

    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public IActionResult Index()
        {
            return View();
        }
    }
}
