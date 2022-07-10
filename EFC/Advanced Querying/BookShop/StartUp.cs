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

            var input = Console.ReadLine();
            var commands = GetBooksByAgeRestriction(db, input);
            Console.WriteLine(commands);
        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();
            command = command.ToLower();
            command = char.ToUpper(command[0]) + command.Substring(1);
            foreach (var titles in context.Books
                                    .Where(x => x.AgeRestriction == (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command))
                                    .OrderBy(x=>x.Title)
                                    .Select(x => x.Title))
            {
                sb.AppendLine(titles);
            }
            return sb.ToString().TrimEnd();
        }
    }
}
