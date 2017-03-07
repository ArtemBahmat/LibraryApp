using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity.Migrations;

namespace DAL
{
    public enum CriteriaSearch
    {
        Author,
        Genre,
        Year
    }
    public class Repository
    {
        public Repository()
        {
           // InitializeDb();
        }

        private void InitializeDb()
        {
            List<Book> books = GenerateBooks();
            Save(books);

            List<BookRental> rentals = GenerateRentals();
            Save(rentals);
        }


        private static void Save(List<BookRental> rentals)
        {
            if (rentals == null || rentals.Count == 0) return;

            try
            {
                using (LibraryContext db = new LibraryContext())
                {
                    db.Rentals.AddRange(rentals);
                    db.SaveChanges();
                }
            }
            catch { }
            
        }

        private static void Save(List<Book> books)
        {
            if (books == null || books.Count == 0) return;

            try
            {
                using (LibraryContext db = new LibraryContext())
                {
                    db.Books.AddRange(books);
                    db.SaveChanges();
                }
            }
            catch { }
        }

        public void Save(Book book)
        {
            if (book == null) return;

            try
            {
                using (LibraryContext db = new LibraryContext())
                {                   
                    db.Books.AddOrUpdate(book);                    
                    db.SaveChanges();
                }
            }
            catch { }            
        }

        public void Update(Book book)
        {
            if (book == null) return;

            try
            {
                using (LibraryContext db = new LibraryContext())
                {
                    db.Entry(book).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch { }
        }

        public void Save(BookRental rental)
        {
            if (rental == null) return;

            try
            {
                using (LibraryContext db = new LibraryContext())
                {
                    db.Rentals.AddOrUpdate(rental);
                    db.SaveChanges();
                }
            }
            catch { }

        }

        public void ReturnBook(string name, string author)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(author)) return;

            try
            {
                using (LibraryContext db = new LibraryContext())
                {                    
                    Book book = db.Rentals.Where(x => x.ClientName == name).SelectMany(x => x.Books).FirstOrDefault(x => x.Author == author);
                    
                    if (book != null)
                    {                       
                        book.Rental_Id = null;
                        book.Rental.EndDate = DateTime.Now;
                        db.SaveChanges();
                    }
                }
            }
            catch { }            
        }

        public List<Book> GetHistory(string name, bool all)
        {
            List<Book> result = new List<Book>();
            try
            {
                using (LibraryContext db = new LibraryContext())
                {                   
                    if (all)
                    {
                        result = db.Books.Include("Rental").Where(x => x.Rental.ClientName == name ).ToList();
                    }
                    else
                        result = db.Books.Include("Rental").Where(x => x.Rental.ClientName == name && x.Rental.StartDate >= x.Rental.EndDate).ToList();
                    return result;
                }
            }
            catch { }            
            return result;
        }

        public void Delete(Book book)
        {
            if (book == null) return;

            try
            {
                using (LibraryContext db = new LibraryContext())
                {
                    Book b = db.Books.FirstOrDefault(x => x.Author == book.Author && x.Genre == book.Genre && x.Year == book.Year);

                    if (b != null)
                    {
                        db.Books.Remove(b);
                        db.SaveChanges();
                    }
                }
            }
            catch { }
        }

        private List<Book> GetBooks(Expression<Func<Book, bool>> filter)
        {         
            if (filter == null ) return new List<Book>();

            try
            {
                using (LibraryContext db = new LibraryContext())
                {
                    return db.Books.Where(filter).ToList<Book>();
                }
            }
            catch { }

            return new List<Book>();
        }       

        private List<Book> GenerateBooks()
        {
            return new List<Book>()
                {
                new Book() { Author = "Pushkin", Genre = "Poetry", Year =1996 },
                new Book() { Author = "Shakespear", Genre = "Poetry", Year =1986 },
                new Book() { Author = "Tolstoy", Genre = "Proza", Year = 1986 },
                new Book() { Author = "Chexov", Genre = "Proza", Year = 2005 },
                new Book() { Author = "Strugatscky", Genre = "Fantastic", Year = 2005 },
                new Book() { Author = "Suvorov", Genre = "History", Year = 2008 }
                };
        }

        private List<BookRental> GenerateRentals()
        {
            return new List<BookRental>()
            {
                new BookRental() {ClientName = "Ivanov", StartDate = new DateTime(1996, 2, 15), EndDate = DateTime.MinValue },
                new BookRental() {ClientName = "Ivanov", StartDate = new DateTime(1996, 2, 15), EndDate = DateTime.MinValue },              
            };
        }

        public  List<Book> FindBook(CriteriaSearch filter, string value)
        {
            Expression<Func<Book, bool>> condition = null;

            switch (filter)
            {
                case CriteriaSearch.Author:
                    condition = x => x.Author == value;
                    break;
                case CriteriaSearch.Genre:
                    condition = x => x.Genre == value;
                    break;
                case CriteriaSearch.Year:
                    int year;

                    if (int.TryParse(value, out year))
                    {
                        condition = x => x.Year == year;
                    }
                    break;
            }

            List<Book> result = GetBooks(condition);
            return result;
        }     
    }
}

