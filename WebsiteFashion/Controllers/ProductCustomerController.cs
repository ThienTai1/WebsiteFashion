using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using WebsiteFashion.Models;
using WebsiteFashion.Repositories;


namespace WebsiteFashion.Controllers
{

    public class ProductCustomerController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductCustomerController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }


        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync();

            return View(new Tuple<IEnumerable<Product>, IEnumerable<Category>>(products, categories));
        }
        public async Task<IActionResult> AddAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }


        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public async Task<IActionResult> Search(string? query)
        {
            var products = await _productRepository.GetAllAsync();
            if (query != null)
            {
                products = products.Where(p => p.Name.ToUpper().Contains(query.ToUpper()));
            }
            return View(products);
        }

/*        public async Task<IActionResult> SearchByCategory(int categoryId)
        {
            var products = await _productRepository.GetAllAsync(categoryId);
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (products == null || category == null)
            {
                return NotFound();
            }

            var viewModel = new ProductCategoryViewModel
            {
                Category = category,
                Products = products
            };

            return View(viewModel);
        }*/

    }
}
