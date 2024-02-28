using Microsoft.CodeAnalysis.Options;

namespace EpicBookstoreSprint.Models
{
    // Represents an order in the bookstore
    public class Order
    {
        // Unique identifier for the order
        public int Id { get; set; }
        // List of items included in the order
        public List<OrderItem> OrderItems { get; set; } = new();
        // Total cost of the order
        public int OrderTotal { get; set; }
        // Date and time when the order was placed
        public DateTime OrderPlaced { get; set; } 

    }
}
