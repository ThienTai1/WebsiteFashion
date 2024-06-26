﻿using System.Collections.Generic;
using System.Threading.Tasks;
using WebsiteFashion.Models;

namespace WebsiteFashion.Repositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> GetByIdAsync(string userId);
        Task UpdateAsync(ApplicationUser employee);
        Task DeleteAsync(string userId);
    }
}
