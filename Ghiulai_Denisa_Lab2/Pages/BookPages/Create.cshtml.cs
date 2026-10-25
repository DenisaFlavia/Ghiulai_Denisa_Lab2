
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ghiulai_Denisa_Lab2.Models;

namespace Ghiulai_Denisa_Lab2.Pages.BookPages;

public class CreateModel : BookCategoriesPageModel
{
    private readonly Ghiulai_Denisa_Lab2Context _context;

    public CreateModel(Ghiulai_Denisa_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = new Book();

    public SelectList PublisherList { get; set; } = default!;

    public SelectList AuthorList { get; set; } = default!;

    private async Task LoadListsAsync()
    {
        var publishers = await _context.Publisher
            .OrderBy(p => p.PublisherName)
            .ToListAsync();

        PublisherList = new SelectList(
            publishers, "ID", "PublisherName");

        var authors = await _context.Author
            .OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName)
            .ToListAsync();

        var authorOptions = authors.Select(a => new
        {
            a.ID,
            FullName = a.FirstName + " " + a.LastName
        }).ToList();

        AuthorList = new SelectList(
            authorOptions, "ID", "FullName");
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadListsAsync();

        Book = new Book
        {
            BookCategories = new List<BookCategory>()
        };

        PopulateAssignedCategoryData(_context, Book);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(
        string[] selectedCategories)
    {
        ModelState.Remove("Book.Publisher");
        ModelState.Remove("Book.AuthorNavigation");
        ModelState.Remove("Book.BookCategories");

        if (!ModelState.IsValid)
        {
            await LoadListsAsync();
            PopulateAssignedCategoryData(_context, new Book
            {
                BookCategories = new List<BookCategory>()
            });

            foreach (var category in AssignedCategoryDataList)
            {
                category.Assigned = selectedCategories.Contains(
                    category.CategoryID.ToString());
            }

            return Page();
        }

        var selectedAuthor = await _context.Author
            .FirstOrDefaultAsync(a => a.ID == Book.AuthorID);

        if (selectedAuthor == null)
        {
            ModelState.AddModelError(
                "Book.AuthorID",
                "Please select a valid author.");

            await LoadListsAsync();

            PopulateAssignedCategoryData(_context, new Book
            {
                BookCategories = new List<BookCategory>()
            });

            foreach (var category in AssignedCategoryDataList)
            {
                category.Assigned = selectedCategories.Contains(
                    category.CategoryID.ToString());
            }

            return Page();
        }

        var book = new Book
        {
            Title = Book.Title,
            AuthorID = selectedAuthor.ID,
            Price = Book.Price,
            PublishingDate = Book.PublishingDate,
            PublisherID = Book.PublisherID,
            BookCategories = new List<BookCategory>()
        };

        foreach (var categoryID in selectedCategories)
        {
            if (int.TryParse(categoryID, out int id))
            {
                book.BookCategories.Add(new BookCategory
                {
                    CategoryID = id
                });
            }
        }

        _context.Book.Add(book);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
