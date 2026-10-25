
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ghiulai_Denisa_Lab2.Models;

namespace Ghiulai_Denisa_Lab2.Pages.BookPages;

public class EditModel : BookCategoriesPageModel
{
    private readonly Ghiulai_Denisa_Lab2Context _context;

    public EditModel(Ghiulai_Denisa_Lab2Context context)
    {
        _context = context;
    }

    [BindProperty]
    public Book Book { get; set; } = default!;

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

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var book = await _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.AuthorNavigation)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.ID == id);

        if (book is null)
        {
            return NotFound();
        }

        Book = book;

        PopulateAssignedCategoryData(_context, Book);

        await LoadListsAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(
        string[] selectedCategories)
    {
        ModelState.Remove("Book.Publisher");
        ModelState.Remove("Book.AuthorNavigation");
        ModelState.Remove("Book.BookCategories");

        var bookToUpdate = await _context.Book
            .Include(b => b.Publisher)
            .Include(b => b.AuthorNavigation)
            .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(b => b.ID == Book.ID);

        if (bookToUpdate is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            Book.BookCategories = bookToUpdate.BookCategories;

            PopulateAssignedCategoryData(_context, Book);

            foreach (var category in AssignedCategoryDataList)
            {
                category.Assigned =
                    selectedCategories.Contains(
                        category.CategoryID.ToString());
            }

            await LoadListsAsync();
            return Page();
        }

        var selectedAuthor = await _context.Author
            .FirstOrDefaultAsync(a => a.ID == Book.AuthorID);

        if (selectedAuthor is null)
        {
            ModelState.AddModelError(
                "Book.AuthorID",
                "Please select a valid author.");

            Book.BookCategories = bookToUpdate.BookCategories;

            PopulateAssignedCategoryData(_context, Book);

            foreach (var category in AssignedCategoryDataList)
            {
                category.Assigned =
                    selectedCategories.Contains(
                        category.CategoryID.ToString());
            }

            await LoadListsAsync();
            return Page();
        }

        bookToUpdate.Title = Book.Title;
        bookToUpdate.AuthorID = selectedAuthor.ID;
        bookToUpdate.Price = Book.Price;
        bookToUpdate.PublishingDate = Book.PublishingDate;
        bookToUpdate.PublisherID = Book.PublisherID;

        UpdateBookCategories(
            _context,
            selectedCategories,
            bookToUpdate);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            bool exists = await _context.Book
                .AnyAsync(b => b.ID == Book.ID);

            if (!exists)
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToPage("./Index");
    }
}
