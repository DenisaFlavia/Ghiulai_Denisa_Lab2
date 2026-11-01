
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ghiulai_Denisa_Lab2.Models;

namespace Ghiulai_Denisa_Lab2.Pages.CategoryPages;

public class IndexModel : PageModel
{
    private readonly Ghiulai_Denisa_Lab2Context _context;

    public IndexModel(Ghiulai_Denisa_Lab2Context context)
    {
        _context = context;
    }

    public IList<Category> Category { get; set; }
        = new List<Category>();

    public int CategoryID { get; set; }

    public List<Book> SelectedBooks { get; set; }
        = new List<Book>();

    public async Task OnGetAsync(int? id)
    {
        Category = await _context.Category
            .Include(c => c.BookCategories)
                .ThenInclude(bc => bc.Book)
                    .ThenInclude(b => b.AuthorNavigation)
            .OrderBy(c => c.CategoryName)
            .AsNoTracking()
            .ToListAsync();

        if (id != null)
        {
            CategoryID = id.Value;

            var selectedCategory = Category
                .FirstOrDefault(c => c.ID == id.Value);

            if (selectedCategory != null)
            {
                SelectedBooks = selectedCategory.BookCategories?
                    .Select(bc => bc.Book)
                    .Where(b => b != null)
                    .OrderBy(b => b.Title)
                    .ToList() ?? new List<Book>();
            }
        }
    }
}
