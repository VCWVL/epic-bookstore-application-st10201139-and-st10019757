namespace EpicBookstoreSprint.Models
{
    public class CartItems
    {
        public int Id { get; set; }
        public Books books { get; set; }
        public int Quantity { get; set; }
        public string CartId { get; set; }
    }
}
