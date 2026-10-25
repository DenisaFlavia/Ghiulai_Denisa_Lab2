
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ghiulai_Denisa_Lab2.Models;

namespace Ghiulai_Denisa_Lab2.Pages.BookPages;

public class IndexModel : PageModel
{
    private readonly Ghiulai_Denisa_Lab2Context _context;

    public IndexModel(Ghiulai_Denisa_Lab2Context context)
    {
        _context = context;
    }

    public IList<Book> Book { get; set; } = new List<Book>();

    public BookData BookD { get; set; } = new BookData();

    public int BookID { get; set; }

    public int CategoryID { get; set; }

    public async Task OnGetAsync(int? id, int? categoryID)
    {
        BookD = new BookData();

        BookD.Books = await _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.AuthorNavigation)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .AsNoTracking()
            .OrderBy(b => b.Title)
            .ToListAsync();

        Book = BookD.Books.ToList();

        if (id != null)
        {
            BookID = id.Value;

            var book = BookD.Books
                .FirstOrDefault(b => b.ID == id.Value);

            if (book != null)
            {
                BookD.Categories = book.BookCategories?
                    .Select(bc => bc.Category)
                    .ToList() ?? new List<Category>();
            }
        }

        if (categoryID != null)
        {
            CategoryID = categoryID.Value;
        }
    }
}
