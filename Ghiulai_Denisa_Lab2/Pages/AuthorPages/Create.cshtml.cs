
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ghiulai_Denisa_Lab2.Models;

namespace Ghiulai_Denisa_Lab2.Pages.AuthorPages;

public class CreateModel : PageModel
{
    private readonly Ghiulai_Denisa_Lab2Context _context;

    public CreateModel(Ghiulai_Denisa_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Author Author { get; set; } = new Author();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Author.Add(Author);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
