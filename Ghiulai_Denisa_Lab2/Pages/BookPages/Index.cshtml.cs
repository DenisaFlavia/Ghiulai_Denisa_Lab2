
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

    public string TitleSort { get; set; } = string.Empty;

    public string AuthorSort { get; set; } = string.Empty;

    public string CurrentFilter { get; set; } = string.Empty;

    public async Task OnGetAsync(
        int? id,
        int? categoryID,
        string? sortOrder,
        string? searchString)
    {
        BookD = new BookData();

        TitleSort = string.IsNullOrEmpty(sortOrder)
            ? "title_desc" : "";

        AuthorSort = sortOrder == "author"
            ? "author_desc" : "author";

        CurrentFilter = searchString ?? string.Empty;

        BookD.Books = await _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.AuthorNavigation)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .AsNoTracking()
            .OrderBy(b => b.Title)
            .ToListAsync();

        if (!string.IsNullOrEmpty(searchString))
        {
            BookD.Books = BookD.Books.Where(b =>
                b.Title.Contains(searchString) ||
                (b.AuthorNavigation != null &&
                (b.AuthorNavigation.FirstName.Contains(searchString) ||
                 b.AuthorNavigation.LastName.Contains(searchString))));
        }

        if (id != null)
        {
            BookID = id.Value;

            var book = BookD.Books
                .FirstOrDefault(b => b.ID == id.Value);

            if (book != null)
            {
                BookD.Categories = book.BookCategories
                    .Select(bc => bc.Category)
                    .ToList();
            }
        }

        if (categoryID != null)
        {
            CategoryID = categoryID.Value;
        }

        switch (sortOrder)
        {
            case "title_desc":
                BookD.Books = BookD.Books
                    .OrderByDescending(b => b.Title);
                break;

            case "author_desc":
                BookD.Books = BookD.Books
                    .OrderByDescending(b =>
                        b.AuthorNavigation?.FullName);
                break;

            case "author":
                BookD.Books = BookD.Books
                    .OrderBy(b =>
                        b.AuthorNavigation?.FullName);
                break;

            default:
                BookD.Books = BookD.Books
                    .OrderBy(b => b.Title);
                break;
        }

        Book = BookD.Books.ToList();
    }
}
