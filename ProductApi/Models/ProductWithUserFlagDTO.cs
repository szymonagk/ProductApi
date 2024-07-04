using ProductApi.Entities;

namespace ProductApi.Models
{
    public class ProductWithUserFlagDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ProductImage> Images { get; set; }
        public List<Variant> Variants { get; set; }
        public int TimesFlagged { get; set; }
        public bool IsFlaggedByUser { get; set; }
    }
}
