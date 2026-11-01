
using Ghiulai_Denisa_Lab2.Models;

namespace Ghiulai_Denisa_Lab2.ViewModels
{
    public class PublisherIndexData
    {
        public IEnumerable<Publisher> Publishers { get; set; }
            = new List<Publisher>();

        public IEnumerable<Book> Books { get; set; }
            = new List<Book>();
    }
}
