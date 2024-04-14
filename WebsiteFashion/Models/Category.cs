using System.ComponentModel.DataAnnotations;

namespace WebsiteFashion.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public required string Name { get; set; }
        public List<Product>? Products { get; set; }

        public Coupon? Coupon { get; set; }
    }
}
