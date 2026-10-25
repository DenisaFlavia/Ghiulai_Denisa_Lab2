
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ghiulai_Denisa_Lab2.Models;

namespace Ghiulai_Denisa_Lab2.Pages.AuthorPages;

public class DetailsModel : PageModel
{
    private readonly Ghiulai_Denisa_Lab2Context _context;

    public DetailsModel(Ghiulai_Denisa_Lab2Context context)
    {
        _context = context;
    }

    public Author Author { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var author = await _context.Author
            .FirstOrDefaultAsync(a => a.ID == id);

        if (author == null)
        {
            return NotFound();
        }

        Author = author;
        return Page();
    }
}
