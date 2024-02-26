namespace EpicBookstoreSprint.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public Order order { get; set; }
        public Books books { get; set; }

    }
}
