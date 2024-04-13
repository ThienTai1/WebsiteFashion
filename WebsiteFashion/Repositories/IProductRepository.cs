namespace WebsiteFashion.Repositories

{
    using System.Collections.Generic;
    using WebsiteFashion.Models;
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}
