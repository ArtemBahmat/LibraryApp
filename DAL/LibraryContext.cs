using DAL.Model;
using System.Data.Entity;

namespace DAL
{
    class LibraryContext : DbContext
    {
        public LibraryContext() : base("DbConnection")
        {          
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookRental> Rentals { get; set; }
    }
}
