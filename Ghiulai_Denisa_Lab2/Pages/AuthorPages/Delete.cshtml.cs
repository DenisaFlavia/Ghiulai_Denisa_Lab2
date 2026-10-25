
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ghiulai_Denisa_Lab2.Models;

namespace Ghiulai_Denisa_Lab2.Pages.AuthorPages;

public class DeleteModel : PageModel
{
    private readonly Ghiulai_Denisa_Lab2Context _context;

    public DeleteModel(Ghiulai_Denisa_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Author Author { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var author = await _context.Author
            .FirstOrDefaultAsync(a => a.ID == id);

        if (author == null)
            return NotFound();

        Author = author;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null)
            return NotFound();

        var author = await _context.Author
            .FirstOrDefaultAsync(a => a.ID == id);

        if (author == null)
            return NotFound();

        bool hasBooks = await _context.Book
            .AnyAsync(b => b.AuthorID == id);

        if (hasBooks)
        {
            Author = author;
            ModelState.AddModelError(
                string.Empty,
                "This author has associated books and cannot be deleted.");
            return Page();
        }

        _context.Author.Remove(author);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
