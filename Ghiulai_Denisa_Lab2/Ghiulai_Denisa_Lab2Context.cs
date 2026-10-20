
using Microsoft.EntityFrameworkCore;
using Ghiulai_Denisa_Lab2.Models;

namespace Ghiulai_Denisa_Lab2
{
    public class Ghiulai_Denisa_Lab2Context : DbContext
    {
        public Ghiulai_Denisa_Lab2Context(
            DbContextOptions<Ghiulai_Denisa_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Book> Book { get; set; } = default!;

         public DbSet<Publisher> Publisher { get; set; } = default!;
    }
}
