
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ghiulai_Denisa_Lab2.Models;

namespace Ghiulai_Denisa_Lab2.Pages.AuthorPages;

public class EditModel : PageModel
{
    private readonly Ghiulai_Denisa_Lab2Context _context;

    public EditModel(Ghiulai_Denisa_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Author Author { get; set; } = new Author();

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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var authorToUpdate = await _context.Author
            .FirstOrDefaultAsync(a => a.ID == Author.ID);

        if (authorToUpdate == null)
            return NotFound();

        authorToUpdate.FirstName = Author.FirstName;
        authorToUpdate.LastName = Author.LastName;

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
