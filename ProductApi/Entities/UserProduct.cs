namespace ProductApi.Entities
{
    public class UserProduct
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public bool IsFlagged { get; set; } = false;
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
