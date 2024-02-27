namespace EpicBookstoreSprint.Models
{
    // Represents an item in the shopping cart
    public class CartItems
    {
        // Unique identifier for the cart item
        public int Id { get; set; }
        // The book associated with the cart item
        public Books books { get; set; }
        // The quantity of the specific book in the cart
        public int Quantity { get; set; }
        // Identifier of the cart to which this item belongs
        public string CartId { get; set; }
    }
}
