using EpicBookstoreSprint.Data;
using Microsoft.EntityFrameworkCore;

namespace EpicBookstoreSprint.Models
{
    public class Cart
    {
        private readonly EpicBookstoreContext _context;

        public Cart(EpicBookstoreContext context)
        {
            _context = context;
        }
        public string Id { get; set; }
        public List<CartItems> cartItems { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = services.GetService<EpicBookstoreContext>();
            string cartId = session.GetString("Id") ?? Guid.NewGuid().ToString();

            session.SetString("Id", cartId);


            return new Cart(context) { Id = cartId };
        }

        public CartItems GetCart(Books book) 
        {
            return _context.CartItem.SingleOrDefault(ci =>
              ci.books.Id == book.Id && ci.CartId ==  Id);
        }

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


        public void RemoveFromCart(Books books)
        {
            var cartItem = GetCart(books);

            if (cartItem != null)
            {
                _context.CartItem.Remove(cartItem);
            }

            _context.SaveChanges();
        }

        public void ClearCart()
        {
            var cartItems = _context.CartItem.Where(ci => ci.CartId == Id);

            _context.CartItem.RemoveRange(cartItems);

            _context.SaveChanges();
        }

        public List<CartItems> GetCartItems()
        {
            return cartItems ?? (cartItems = _context.CartItem
                .Where(ci => ci.CartId == Id)
                .Include(ci => ci.books)
                .ToList());
        }

        public int GetCartTotal()
        {
            return _context.CartItem
                .Where(ci => ci.CartId == Id)
                .Select(ci => ci.books.Price * ci.Quantity)
                .Sum();

        }
    }
}
