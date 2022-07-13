namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //  var input = Console.ReadLine();
            //var commands = GetBooksByAgeRestriction(db, input);
            //var commands = GetGoldenBooks(db);
            //var commands = GetBooksByPrice(db);
            //int year = int.Parse(Console.ReadLine());
            //var words = Console.ReadLine();
            //var commands = GetBooksByCategory(db, words);
            //var date = Console.ReadLine();
            //var commands = GetBooksReleasedBefore(db, date);
            var input = Console.ReadLine();
            var commands = GetAuthorNamesEndingIn(db, input);
            Console.WriteLine(commands);
        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();
            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);
            var titles = context.Books
                                    .Where(x => x.AgeRestriction == ageRestriction)
                                    .OrderBy(x => x.Title)
                                    .Select(x => x.Title);
            foreach (var title in titles)
            {
                sb.AppendLine(title);
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            string edition = string.Empty;
            var books = context.Books
                .Where(x => (int)x.EditionType == 2 && x.Copies < 5000)
                .ToList();
            foreach (var book in books.OrderBy(x => x.BookId))
            {
                sb.AppendLine(book.Title);
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(x => x.Price > 40)
                .ToList();

            foreach (var book in books.OrderByDescending(x => x.Price))
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .ToList();

            foreach (var book in books.OrderBy(x => x.BookId))
            {
                sb.AppendLine(book.Title);
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var words = input.Split(" ").Select(x => x.ToLower()).ToList();
            var books = context.BooksCategories
                .Where(x => words.Contains(x.Category.Name.ToLower()))
                .Select(x => x.Book.Title)
                .OrderBy(x => x)
                .ToList();
            return String.Join("\n", books);
        }
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();
            var books = context.Books
                .Where(x => x.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture))
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => new
                {
                    Title = x.Title,
                    Edition = x.EditionType,
                    Price = x.Price
                })
                .ToList();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.Edition} - ${book.Price:f2}");
            }
            return sb.ToString().TrimEnd();
        }
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var sqlPattern = $"%{input}";
            var authors = context.Authors
                .Where(x => EF.Functions.Like(x.FirstName, sqlPattern))
                .Select(x => $"{x.FirstName} {x.LastName}")
                .ToList();
            return String.Join(Environment.NewLine, authors.OrderBy(x => x));
        }
    }
}
