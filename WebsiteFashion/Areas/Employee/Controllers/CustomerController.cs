﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebsiteFashion.Data;
using WebsiteFashion.Models;
using WebsiteFashion.Repositories;

namespace WebsiteFashion.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = SD.Role_Employee)] // Adjust the role as necessary
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public CustomerController(ICustomerRepository customerRepository, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _customerRepository = customerRepository;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchString)
        {

            var customer = await _userManager.GetUsersInRoleAsync("Customer");
            var customerIds = customer.Select(u => u.Id);
            var allCustomer = from s in _context.Customer
                              where customerIds.Contains(s.Id)
                              select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                string lowercaseSearchString = searchString.ToLower();
                allCustomer = allCustomer.Where(s => s.FullName.ToLower().Contains(lowercaseSearchString));
            }

            return View(await allCustomer.ToListAsync());
        }

        // GET: Displays the form to update an Customer
        public async Task<IActionResult> Edit(string id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Updates an Customer
        [HttpPost]
        public async Task<IActionResult> Edit(string id, ApplicationUser customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingCustomer = await _customerRepository.GetByIdAsync(id); // Giả định có phương thức GetByIdAsync

                existingCustomer.FullName = customer.FullName;
                existingCustomer.UserName = customer.UserName;
                existingCustomer.Email = customer.Email;

                await _customerRepository.UpdateAsync(existingCustomer);

                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Displays the confirmation page for deleting an Customer
        public async Task<IActionResult> Delete(string id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Deletes an Customer
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _customerRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
