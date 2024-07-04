namespace ProductApi.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ProductImage> Images { get; set; }
        public List<Variant> Variants { get; set; }
        public int TimesFlagged { get; set; } = 0;
    }

    public class Variant
    {
        public int Id { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? Weight { get; set; }
        public string? Dimensions { get; set; }
    }

    public class ProductImage
    {
        public int Id { get; set; }
        public string? Url { get; set; }
    }
}
