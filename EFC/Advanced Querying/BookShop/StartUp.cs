namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System;
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
            var commands = GetGoldenBooks(db);
            commands = GetBooksByPrice(db);
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



            return sb.ToString().TrimEnd();
        }
    }
}
