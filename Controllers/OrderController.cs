using EpicBookstoreSprint.Data;
using EpicBookstoreSprint.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpicBookstoreSprint.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly EpicBookstoreContext _context;
        private readonly Cart _cart;
        // Constructor to inject the database context and Cart service
        public OrderController(EpicBookstoreContext context, Cart cart )
        {
            _context = context;
            _cart = cart;
            
        }
        // Display the checkout form
        public IActionResult Checkout()
        {
            return View();
        }
        // Process the checkout form submission
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
           var cartItems = _cart.GetCartItems();
            _cart.cartItems = cartItems;
            // If the cart is empty, display an error message
            if (_cart.cartItems.Count == 0)
            {
                ModelState.AddModelError("","Cart is empty, please add a book first");

            }

            // If the form is valid, create an order and redirect to the checkout complete view

            if (ModelState.IsValid)
            {
                CreateOrder(order);
                _cart.ClearCart();
                return View("CheckoutComplete", order);
            }
            // If the form is not valid, redisplay the checkout form with validation errors

            return View(order);
        }
        // Display the checkout complete view
        public IActionResult CheckoutComplete(Order order)
        {
            return View(order);
        }
        // Create an order based on the items in the cart
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
