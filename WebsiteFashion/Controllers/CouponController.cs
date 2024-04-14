using Microsoft.AspNetCore.Mvc;
using WebsiteFashion.Repositories;
using WebsiteFashion.Models;
using WebsiteFashion.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebsiteFashion.Controllers
{
    public class CouponController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICouponRepository _couponRepository;
        private readonly ICategoryRepository _categoryRepository;

        public CouponController(ICouponRepository couponRepository, ApplicationDbContext context, ICategoryRepository categoryRepository)
        {
            _couponRepository = couponRepository;
            _context = context;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var coupons = await _couponRepository.GetAllAsync();
            return View(coupons);
        }


        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> AddAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin, Employee")]
        [HttpPost]
        public async Task<IActionResult> Add(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                await _couponRepository.AddAsync(coupon);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(coupon);
        }
    }
}
