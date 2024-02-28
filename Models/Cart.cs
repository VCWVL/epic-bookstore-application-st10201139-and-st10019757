using EpicBookstoreSprint.Data;
using Microsoft.EntityFrameworkCore;

namespace EpicBookstoreSprint.Models
{
    // Represents a shopping cart for books
    public class Cart
    {
        private readonly EpicBookstoreContext _context;
        // Constructor that initializes the Cart with a context
        public Cart(EpicBookstoreContext context)
        {
            _context = context;
        }
        // Unique identifier for the cart
        public string Id { get; set; }
        // List of cart items representing the books in the cart
        public List<CartItems> cartItems { get; set; }
        // Static method to retrieve the current cart using session and context
        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = services.GetService<EpicBookstoreContext>();
            string cartId = session.GetString("Id") ?? Guid.NewGuid().ToString();

            session.SetString("Id", cartId);


            return new Cart(context) { Id = cartId };
        }
        // Get a specific cart item based on the associated book
        public CartItems GetCart(Books book) 
        {
            return _context.CartItem.SingleOrDefault(ci =>
              ci.books.Id == book.Id && ci.CartId ==  Id);
        }
        // Add a book to the cart or update quantity if already in the cart
        public void AddToCart(Books books, int quantity)
        {
            var cartItem = GetCart(books);
            if (cartItem == null)
            {
                cartItem = new CartItems
                {
                    books = books,
                    Quantity = quantity,
                    CartId = Id
                };

                _context.CartItem.Add(cartItem);
            }
            else
            {
                   cartItem.Quantity += quantity;
            }
            _context.SaveChanges();
        }
        // Reduce the quantity of a specific book in the cart
        public int ReduceQuantity(Books books)
        {
            var items = GetCart(books);
            var remainingQuantity = 0;

            if (items != null)
            {
                if (items.Quantity > 1)
                {
                    remainingQuantity = --items.Quantity;
                }
                else
                {
                    _context.CartItem.Remove(items);
                }

            }
            _context.SaveChanges();

            return remainingQuantity;
        }

        // Increase the quantity of a specific book in the cart
        public int IncreaseQuantity(Books books)
        {
            var items = GetCart(books);
            var newQuantity = 0;

            if (items != null)
            {
                newQuantity = items.Quantity + 1;
                items.Quantity = newQuantity;
                _context.SaveChanges();
            }

            return newQuantity;
        }

        // Remove a specific book from the cart
        public void RemoveFromCart(Books books)
        {
            var cartItem = GetCart(books);

            if (cartItem != null)
            {
                _context.CartItem.Remove(cartItem);
            }

            _context.SaveChanges();
        }
        // Clear all items from the cart
        public void ClearCart()
        {
            var cartItems = _context.CartItem.Where(ci => ci.CartId == Id);

            _context.CartItem.RemoveRange(cartItems);

            _context.SaveChanges();
        }
        // Get all cart items for display or further processing
        public List<CartItems> GetCartItems()
        {
            return cartItems ?? (cartItems = _context.CartItem
                .Where(ci => ci.CartId == Id)
                .Include(ci => ci.books)
                .ToList());
        }
        // Calculate and return the total price of all items in the cart
        public int GetCartTotal()
        {
            return _context.CartItem
                .Where(ci => ci.CartId == Id)
                .Select(ci => ci.books.Price * ci.Quantity)
                .Sum();

        }
    }
}
