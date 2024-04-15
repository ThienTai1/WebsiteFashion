namespace WebsiteFashion.Models
{
    public class ProductCategoryViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Coupon> Coupons { get; set; }
    }
}
