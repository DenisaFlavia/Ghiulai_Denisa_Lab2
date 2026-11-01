
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ghiulai_Denisa_Lab2.Models;
using Ghiulai_Denisa_Lab2.ViewModels;

namespace Ghiulai_Denisa_Lab2.Pages.PublisherPages;

public class IndexModel : PageModel
{
    private readonly Ghiulai_Denisa_Lab2Context _context;

    public IndexModel(Ghiulai_Denisa_Lab2Context context)
    {
        _context = context;
    }

    public IList<Publisher> Publisher { get; set; }
        = new List<Publisher>();

    public PublisherIndexData PublisherData { get; set; }
        = new PublisherIndexData();

    public int PublisherID { get; set; }

    public int BookID { get; set; }

    public async Task OnGetAsync(int? id, int? bookID)
    {
        PublisherData = new PublisherIndexData();

        PublisherData.Publishers = await _context.Publisher
            .Include(p => p.Books)
                .ThenInclude(b => b.AuthorNavigation)
            .OrderBy(p => p.PublisherName)
            .ToListAsync();

        Publisher = PublisherData.Publishers.ToList();

        if (id != null)
        {
            PublisherID = id.Value;

            var publisher = PublisherData.Publishers
                .FirstOrDefault(p => p.ID == id.Value);

            if (publisher != null)
            {
                PublisherData.Books = publisher.Books
                    ?? new List<Book>();
            }
        }

        if (bookID != null)
        {
            BookID = bookID.Value;
        }
    }
}
