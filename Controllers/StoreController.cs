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
        public async Task<IActionResult> Index()
        {
            return _context.Book != null ?
                        View(await _context.Book.ToListAsync()) :
                        Problem("Entity set 'EpicBookstoreContext.Book'  is null.");
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
