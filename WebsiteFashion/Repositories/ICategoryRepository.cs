using Microsoft.AspNetCore.Identity;

namespace WebsiteFashion.Repositories
{
    using System.Collections.Generic;
    using WebsiteFashion.Models;
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}
