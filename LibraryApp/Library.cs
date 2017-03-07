using DAL;
using DAL.Model;
using System;
using System.Collections.Generic;

namespace LibraryApp
{
    public class Library
    {
        private const string  START_WORD = @"Please select select option: 1 - Find Book, 2 - Add book, 3 - Remove Book, 4 - exit";
        private const string FIND_BOOK_OPTION = @"5 - Author, 6 - Genre, 7 - Year";
        private const string FIND_AUTHOR = @"Please enter author";
        private const string FIND_GENRE = @"Please enter genre";
        private const string FIND_YEAR = @"Please enter year";

        private static Repository libraryRepository { get; } = new Repository();

        static void Main(string[] args)
        {                                    
            InputData(START_WORD);
            Console.ReadLine();    
        }

        private static string InputData(string option)
        {
            string input = string.Empty;
            Console.WriteLine(option);
            input = Console.ReadLine();
            ProcessInput(input);
            return input;
        }

        private static void PrintBooks(List<Book> books)
        {
            if (books == null || books.Count == 0) return;

            Console.WriteLine("Search result:");
            foreach(Book book in books)
            {
                Console.WriteLine($"{book.Author} {book.Genre} {book.Year}");
            }
        }

        private static Book InputBook()
        {
            Console.WriteLine("Please enter Author: ");
            string author = Console.ReadLine();

            Console.WriteLine("Please enter genre: ");
            string genre = Console.ReadLine();

            Console.WriteLine("Please enter year: ");
            string year = Console.ReadLine();

            if (string.IsNullOrEmpty(author) || string.IsNullOrEmpty(genre) || string.IsNullOrEmpty(year))
            {
                Console.WriteLine("Wrong input...");
                return null;
            }

           return new Book() { Author = author, Genre = genre, Year = Convert.ToInt32(year) };
        }

        private static  void ProcessInput(string option)
        {
            if (string.IsNullOrEmpty(option)) return;

            switch(option)
            {
                case "1":
                    InputData(FIND_BOOK_OPTION);
                    break;
                case "2":
                    Book book = InputBook();
                    libraryRepository.Save(book);
                    Console.WriteLine("Book was ssuccessfully saved");                    
                    break;
                case "3":
                    book = InputBook();
                    libraryRepository.Delete(book);
                    Console.WriteLine("Book was ssuccessfully deleted");                    
                    break;
                case "4":
                    Environment.Exit(0);                    
                    break;
                case "5":
                    string author = InputData(FIND_AUTHOR);

                    if (!string.IsNullOrEmpty(author))
                    {
                        List<Book> result = libraryRepository.FindBook(CriteriaSearch.Author, author);
                        PrintBooks(result);
                    }                    
                    break;
                case "6":
                    string genre = InputData(FIND_GENRE);

                    if (!string.IsNullOrEmpty(genre))
                    {
                        List<Book> result = libraryRepository.FindBook(CriteriaSearch.Genre, genre);
                        PrintBooks(result);                        
                    }                    
                    break;
                case "7":
                    string year = InputData(FIND_YEAR);

                    if (!string.IsNullOrEmpty(year))
                    {
                        List<Book> result = libraryRepository.FindBook(CriteriaSearch.Year, year);
                        PrintBooks(result);
                    }                    
                    break;               
            }
            InputData(START_WORD);
        }
    }
}
