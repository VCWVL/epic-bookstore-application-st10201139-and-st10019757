using EpicBookstoreSprint.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EpicBookstoreSprint.Controllers
{
    public class StoreController : Controller
    {
       private readonly EpicBookstoreContext _context;

        public StoreController(EpicBookstoreContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string SearchString, string minPrice, string maxPrice)
        {
            var books = _context.Book.Select(b => b);

            if (!string.IsNullOrEmpty(SearchString))
            {
                books = books.Where(b => b.Title.Contains(SearchString) || b.Author.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(minPrice))
            {
                var min = int.Parse(minPrice);
                books = books.Where(b => b.Price >= min);
            }

            if (!string.IsNullOrEmpty(maxPrice))
            {
                var max = int.Parse(maxPrice);
                books = books.Where(b => b.Price <= max);
            }

            var filteredBooks = await books.ToListAsync();

            return filteredBooks != null ? View(filteredBooks) : Problem("Entity set 'EpicBookstoreContext.Book' is null.");
        }


        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var books = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (books == null)
            {
                return NotFound();
            }

            return View(books);
        }
    }
}
