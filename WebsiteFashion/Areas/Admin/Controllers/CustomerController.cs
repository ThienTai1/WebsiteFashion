using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebsiteFashion.Data;
using WebsiteFashion.Models;
using WebsiteFashion.Repositories;

namespace WebsiteFashion.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)] // Adjust the role as necessary
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmployeeRepository _customerRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public CustomerController(IEmployeeRepository employeeRepository, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _customerRepository = employeeRepository;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchString)
        {

            var employee = await _userManager.GetUsersInRoleAsync("Customer");
            var employeeIds = employee.Select(u => u.Id);
            var allEmployee = from s in _context.Employee
                               where employeeIds.Contains(s.Id)
                               select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                string lowercaseSearchString = searchString.ToLower();
                allEmployee = allEmployee.Where(s => s.FullName.ToLower().Contains(lowercaseSearchString));
            }

            return View(await allEmployee.ToListAsync());
        }

        // GET: Displays the form to update an employee
        public async Task<IActionResult> Edit(string id)
        {
            var employee = await _customerRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Updates an employee
        [HttpPost]
        public async Task<IActionResult> Edit(string id, ApplicationUser employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingEmployee = await _customerRepository.GetByIdAsync(id); // Giả định có phương thức GetByIdAsync

                existingEmployee.FullName = employee.FullName;
                existingEmployee.UserName = employee.UserName;
                existingEmployee.Email = employee.Email;

                await _customerRepository.UpdateAsync(existingEmployee);

                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Displays the confirmation page for deleting an employee
        public async Task<IActionResult> Delete(string id)
        {
            var employee = await _customerRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Deletes an employee
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _customerRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}