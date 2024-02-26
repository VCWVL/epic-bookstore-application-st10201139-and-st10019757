using EpicBookstoreSprint.Data;
using EpicBookstoreSprint.Models;
using Microsoft.AspNetCore.Mvc;

namespace EpicBookstoreSprint.Controllers
{
    public class OrderController : Controller
    {
        private readonly EpicBookstoreContext _context;
        private readonly Cart _cart;

        public OrderController(EpicBookstoreContext context, Cart cart )
        {
            _context = context;
            _cart = cart;
            
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
           var cartItems = _cart.GetCartItems();
            _cart.cartItems = cartItems;

            if (_cart.cartItems.Count == 0)
            {
                ModelState.AddModelError("","Cart is empty, please add a book first");

            }

            if (ModelState.IsValid)
            {
                CreateOrder(order);
                _cart.ClearCart();
                return View("CheckoutComplete", order);
            }

            return View(order);
        } 

        public IActionResult CheckoutComplete(Order order)
        {
            return View(order);
        }

        public void CreateOrder(Order order)
        {
            order.OrderPlaced = DateTime.Now;

            var CartItems = _cart.cartItems;

            foreach (var item in CartItems)
            {
                var orderItem = new OrderItem
                {
                    Quantity = item.Quantity,
                    OrderId = order.Id,
                    // Instead of setting BookId directly, set the navigation property.
                    books = item.books,
                    // Calculate the Price based on the current item.
                    Price = item.books.Price * item.Quantity
                };

                order.OrderItems.Add(orderItem);
                order.OrderTotal += orderItem.Price;
            }

            _context.Orders.Add(order);
            _context.SaveChanges();
        }


    }
}
