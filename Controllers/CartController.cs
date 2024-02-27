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

        public CartController(EpicBookstoreContext context,Cart cart)
        {
            _context = context;
            _cart = cart;
        }


        public IActionResult Index()
        {
            var items = _cart.GetCartItems();
            _cart.cartItems = items;
            return View(_cart);
        }

        public IActionResult  AddToCart(int id)
        {
            var selectedbook = GetBookById(id);

            if (selectedbook != null)
            {
                _cart.AddToCart(selectedbook, 1);
            }
            return RedirectToAction("Index", "Store");
        }

       
          public IActionResult RemoveFromCart(int id)
        {
            var selectedBook = GetBookById(id); 

            if (selectedBook != null)
            {
                _cart.RemoveFromCart(selectedBook);

            }

            return RedirectToAction("Index");
        }
      
         public IActionResult ReduceQuantity(int id)
        {
            var selectedBook = GetBookById(id);

            if (selectedBook != null)
            {
                _cart.ReduceQuantity(selectedBook);

            }

            return RedirectToAction("Index");
        }

        public IActionResult IncreaseQuantity(int id)
        {
            var selectedBook = GetBookById(id);

            if (selectedBook != null)
            {
                _cart.IncreaseQuantity(selectedBook);

            }

            return RedirectToAction("Index");
        }

        public IActionResult ClearCart()
        {
            _cart.ClearCart();

            return RedirectToAction("Index");
        }


        public Books GetBookById(int id)
        {
            return _context.Book.FirstOrDefault(b => b.Id == id);
        }
    }
}
