
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ghiulai_Denisa_Lab2.Models;

namespace Ghiulai_Denisa_Lab2.Pages.AuthorPages;

public class IndexModel : PageModel
{
    private readonly Ghiulai_Denisa_Lab2Context _context;

    public IndexModel(Ghiulai_Denisa_Lab2Context context)
    {
        _context = context;
    }

    public IList<Author> Authors { get; set; } = new List<Author>();

    public async Task OnGetAsync()
    {
        Authors = await _context.Author
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .ToListAsync();
    }
}
