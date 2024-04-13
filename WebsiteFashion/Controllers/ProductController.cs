using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using WebsiteFashion.Models;
using WebsiteFashion.Repositories;
using WebsiteFashion.Data;
using Microsoft.EntityFrameworkCore;


namespace WebsiteFashion.Controllers
{
    

    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _context = context;
        }

        public async Task<IActionResult> Index(string? searchString, string? categoryName)
        {
            var allProduct = from p in _context.Products select p;
            var allCategory = from c in _context.Categories select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                string lowercaseSearchString = searchString.ToLower();
                allProduct = allProduct.Where(p => p.Name.ToLower().Contains(lowercaseSearchString));
            }

            if (!string.IsNullOrEmpty(categoryName))
            {
                allProduct = allProduct.Where(p => p.Category.Name.ToLower() == categoryName.ToLower());
            }

            var products = await allProduct.ToListAsync();
            var categories = await allCategory.ToListAsync();

            return View(new Tuple<IEnumerable<Product>, IEnumerable<Category>>(products, categories));
        }


        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> AddAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }
        
        // Add product
        [Authorize(Roles = "Admin, Employee")]
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null)
                {
                    // Lưu hình ảnh đại diện
                    product.ImageUrl = await SaveImage(imageUrl);
                }

                if (imageUrls != null)
                {
                    product.ImageUrls = new List<string>();
                    foreach (var file in imageUrls)
                    {
                        // Lưu các hình ảnh khác
                        product.ImageUrls.Add(await SaveImage(file));
                    }
                }

                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName); // Thay đổi đường dẫn theo cấu hình của bạn
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName; // Trả về đường dẫn tương đối
        }
        private async Task<Product> GetProductFromDatabase(int productId)
        {
            // Truy vấn cơ sở dữ liệu để lấy thông tin sản phẩm
            var product = await _productRepository.GetByIdAsync(productId);
            return product;
        }

        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Display(int productId)
        {
            var product = await GetProductFromDatabase(productId);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }



        // Hiển thị form cập nhật sản phẩm
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                await _productRepository.UpdateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        // Hiển thị form xác nhận xóa sản phẩm
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> DeleteAll()
        {
            var products = await _productRepository.GetAllAsync();
            if (products == null)
            {
                return NotFound();
            }
            else
            {
                foreach (var product in products)
                {
                    await _productRepository.DeleteAsync(product.Id);
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
