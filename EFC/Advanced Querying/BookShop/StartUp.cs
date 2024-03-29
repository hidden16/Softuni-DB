﻿namespace BookShop
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
            //var input = Console.ReadLine();
            //var commands = GetAuthorNamesEndingIn(db, input);
            // var commands = GetBookTitlesContaining(db, input);
            // var commands = GetBooksByAuthor(db, input);
            //var commands = GetBooksByAuthor(db, input);
            //var input = int.Parse(Console.ReadLine());
            //var commands = CountBooks(db, input);
            //var commands = CountCopiesByAuthor(db);
            //var commands = GetTotalProfitByCategory(db);
            //var commands = GetMostRecentBooks(db);
            //IncreasePrices(db);
            int commands = RemoveBooks(db);
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
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            input = input.ToLower();
            var titles = context.Books
                .Where(x => x.Title.ToLower().Contains(input))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();
            return String.Join(Environment.NewLine, titles);
        }
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var pattern = $"{input.ToLower()}%";
            var titlesAuthors = context.Books
                .Include(x => x.Author)
                .Where(x => EF.Functions.Like(x.Author.LastName, pattern))
                .OrderBy(x => x.BookId)
                .Select(x => $"{x.Title} ({x.Author.FirstName} {x.Author.LastName})")
                .ToList();
            return String.Join(Environment.NewLine, titlesAuthors);
        }
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var booksNumber = context.Books
                .Where(x => x.Title.Length > lengthCheck)
                .ToList();
            return booksNumber.Count;
        }
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authorCopies = context.Authors
                .Include(x => x.Books)
                .OrderByDescending(x => x.Books.Select(x => x.Copies).Sum())
                .Select(x => $"{x.FirstName} {x.LastName} - {x.Books.Select(x => x.Copies).Sum()}")
                .ToList();
            return String.Join(Environment.NewLine, authorCopies);
        }
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var categoryProfit = context.Categories
                .Include(x=>x.CategoryBooks)
                .OrderByDescending(x=> x.CategoryBooks.Select(x => x.Book.Copies * x.Book.Price).Sum())
                .ThenBy(x=>x.Name)
                .Select(x => $"{x.Name} ${x.CategoryBooks.Select(x => x.Book.Copies * x.Book.Price).Sum():f2}")
                .ToList();
           
            return String.Join(Environment.NewLine, categoryProfit);
        }
        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var categoryBooks = context.Categories
                .Include(x => x.CategoryBooks)
                .Select(x=> new
                {
                    Name = x.Name,
                    Movies = x.CategoryBooks.OrderByDescending(x => x.Book.ReleaseDate).Select(x=> $"{x.Book.Title} ({x.Book.ReleaseDate.Value.Year})").Take(3)
                })
                .ToList();
            foreach (var category in categoryBooks.OrderBy(x=>x.Name))
            {
                sb.AppendLine($"--{category.Name}");
                foreach (var title in category.Movies)
                {
                    sb.AppendLine($"{title}");
                }
            }
            return sb.ToString().TrimEnd();
        }
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books;
            foreach (var book in books)
            {
                book.Price += 5;
            }
            context.SaveChanges();
        }
        public static int RemoveBooks(BookShopContext context)
        {
            var toDelete = context.Books.Where(x=>x.Copies < 4200).ToList();
            var counter = 0;
            foreach (var item in toDelete)
            {
                counter++;
                context.Books.Remove(item);
            }
            context.SaveChanges();
            return counter;

        }
    }
}
