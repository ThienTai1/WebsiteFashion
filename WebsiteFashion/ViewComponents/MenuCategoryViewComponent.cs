using Microsoft.AspNetCore.Mvc;
using WebsiteFashion.Data;

namespace WebsiteFashion.ViewComponents
{
    public class MenuCategoryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext db;
        public MenuCategoryViewComponent(ApplicationDbContext context) => db = context;
    }
}
