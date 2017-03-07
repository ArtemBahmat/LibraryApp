using DAL;
using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ClientApp
{
    public class ClientApp
    {
        private const string START_WORD = @"Please select select option: 1 - Borrow Book, 2 - Return book, 3 - View borrowed books, 4 - view history, 5 - exit";

        private static Repository libraryRepository { get; } = new Repository();
        public static void Main(string[] args)
        {
            InputData(START_WORD);
            Console.ReadLine();
        }

        private static string InputData(string option)
        {
            Console.WriteLine(option);
            string input = Console.ReadLine();
            ProcessInput(input);
            return input;
        }

        private static Book InputBook()
        {
            Console.WriteLine("Please enter Author: ");
            string author = Console.ReadLine();            

            if (string.IsNullOrEmpty(author))
            {
                Console.WriteLine("Wrong input...");
                return null;
            }

            List<Book> book = libraryRepository.FindBook(CriteriaSearch.Author, author);

            if (book == null || book.Count == 0)
            {
                Console.WriteLine("Nothing found, please try again..");
                return null;
            }
            return book[0];            
        }

        private static void BorrowBook()
        {
            Book book = InputBook();

            if (book == null || book.Rental != null)
            {
                InputData(START_WORD);                
            }
            else
            {
                Console.WriteLine("Please enter your name");
                string name = Console.ReadLine();

                if (!string.IsNullOrEmpty(name))
                {
                    BookRental rental = new BookRental() {ClientName = name, StartDate = DateTime.Now, EndDate = DateTime.MinValue };                                                         
                    libraryRepository.Save(rental);
                    
                    book.Rental_Id = rental.Id;
                    libraryRepository.Update(book);
                }
                else
                {
                    Console.WriteLine("Wrong input");                    
                }
            }
        }

        private static void ReturnBook()
        {
            Console.WriteLine("Please enter your name");
            string name = Console.ReadLine();

            Console.WriteLine("Input book(author)");
            string author = Console.ReadLine();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(author))
            {
                Console.WriteLine("Wrong input");
            }
            else
            {
                libraryRepository.ReturnBook(name, author);
            }

        }

        private static void ViewHistory(bool displayAll)
        {
            Console.WriteLine("Please enter your name");
            string name = Console.ReadLine();

            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Wrong input");
            }
            else
            {
                List<Book> books = libraryRepository.GetHistory(name, displayAll);
                Console.WriteLine("Author       StartDate       EndDate");
                foreach(Book book in books)
                {
                    Console.WriteLine($"{book.Author}     {book.Rental.StartDate.ToShortDateString()}       {book.Rental.EndDate.ToShortDateString()} {Environment.NewLine}");
                }
            }
        }

        private static void ProcessInput(string option)
        {
            if (string.IsNullOrEmpty(option)) return;

            switch (option)
            {
                case "1":
                    BorrowBook();                    
                    break;
                case "2":
                    ReturnBook();                    
                    break;
                case "3":
                    ViewHistory(false);
                    break;
                case "4":
                    ViewHistory(true);                    
                    break;
                case "5":
                    Environment.Exit(0);
                    break;
            }
            InputData(START_WORD);
        }
    }
}
