using EpicBookstoreSprint.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EpicBookstoreSprint.Controllers
{
    [AllowAnonymous]
    public class StoreController : Controller
    {
       
        private readonly EpicBookstoreContext _context;
        // Constructor to inject the database context
        public StoreController(EpicBookstoreContext context)
        {
            _context = context;
        }

        // GET: Books
        // Display the list of books based on search criteria
        public async Task<IActionResult> Index(string SearchString, string minPrice, string maxPrice)
        {
            // Retrieve all books from the database
            var books = _context.Book.Select(b => b);
            // Apply search criteria if provided
            if (!string.IsNullOrEmpty(SearchString))
            {
                books = books.Where(b => b.Title.Contains(SearchString) || b.Author.Contains(SearchString));
            }
            // Apply minimum price filter if provided and valid
            if (!string.IsNullOrEmpty(minPrice) && int.TryParse(minPrice, out var min))
            {
                books = books.Where(b => b.Price >= min);
            }
            // Apply maximum price filter if provided and valid
            if (!string.IsNullOrEmpty(maxPrice) && int.TryParse(maxPrice, out var max))
            {
                books = books.Where(b => b.Price <= max);
            }
            // Retrieve the filtered books and display them in the view
            var filteredBooks = await books.ToListAsync();

            return filteredBooks.Any() ? View(filteredBooks) : Problem("No books found matching the criteria.");

        }

        // GET: Books/Details/5
        // Display details of a specific book
        public async Task<IActionResult> Details(int? id)
            {
            // Check if the book ID is provided and the book entity set is not null

            if (id == null || _context.Book == null)
                {
                    return NotFound();
                }
            // Retrieve the book details from the database based on the provided ID

            var books = await _context.Book
                    .FirstOrDefaultAsync(m => m.Id == id);
            // If the book is not found, return a 404 Not Found response

            if (books == null)
                {
                    return NotFound();
                }
            // Display the details of the book in the view
            return View(books);
            }
        }
    }
