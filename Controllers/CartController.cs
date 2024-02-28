using EpicBookstoreSprint.Data;
using EpicBookstoreSprint.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpicBookstoreSprint.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly EpicBookstoreContext _context;
        private readonly Cart _cart;
        // Constructor to inject the database context and Cart service
        public CartController(EpicBookstoreContext context,Cart cart)
        {
            _context = context;
            _cart = cart;
        }

        // Display the items in the cart
        public IActionResult Index()
        {
            var items = _cart.GetCartItems();
            _cart.cartItems = items;
            return View(_cart);
        }
        // Add a book to the cart
        public IActionResult  AddToCart(int id)
        {
            var selectedbook = GetBookById(id);

            if (selectedbook != null)
            {
                _cart.AddToCart(selectedbook, 1);
            }
            // Redirect to the Store's Index view after adding to the cart
            return RedirectToAction("Index", "Store");
        }

        // Remove a book from the cart
        public IActionResult RemoveFromCart(int id)
        {
            var selectedBook = GetBookById(id); 

            if (selectedBook != null)
            {
                _cart.RemoveFromCart(selectedBook);

            }
            // Redirect to the cart's Index view after removing from the cart
            return RedirectToAction("Index");
        }
        // Reduce the quantity of a book in the cart
        public IActionResult ReduceQuantity(int id)
        {
            var selectedBook = GetBookById(id);

            if (selectedBook != null)
            {
                _cart.ReduceQuantity(selectedBook);

            }
            // Redirect to the cart's Index view after reducing the quantity
            return RedirectToAction("Index");
        }
        // Increase the quantity of a book in the cart
        public IActionResult IncreaseQuantity(int id)
        {
            var selectedBook = GetBookById(id);

            if (selectedBook != null)
            {
                _cart.IncreaseQuantity(selectedBook);

            }

            return RedirectToAction("Index");
        }
        // Clear all items from the cart
        public IActionResult ClearCart()
        {
            _cart.ClearCart();

            return RedirectToAction("Index");
        }

        // Get a book by its id from the database
        public Books GetBookById(int id)
        {
            return _context.Book.FirstOrDefault(b => b.Id == id);
        }
    }
}
