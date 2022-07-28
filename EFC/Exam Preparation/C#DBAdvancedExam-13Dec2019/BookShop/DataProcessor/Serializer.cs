namespace BookShop.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportMostCraziestAuthors(BookShopContext context)
        {
            var authorsJson = context.Authors
                .Select(a => new AuthorExportDto
                {
                    AuthorName = $"{a.FirstName} {a.LastName}",
                    Books = a.AuthorsBooks
                    .Select(x => new AuthorBookExportDto
                    {
                        BookName = x.Book.Name,
                        BookPrice = x.Book.Price.ToString("f2")
                    })
                    .OrderByDescending(x=>decimal.Parse(x.BookPrice))
                    .ToList()
                })
                .ToList()
                .OrderByDescending(x=>x.Books.Count)
                .ThenBy(x=>x.AuthorName)
                .ToList();
            return JsonConvert.SerializeObject(authorsJson);
        }

        public static string ExportOldestBooks(BookShopContext context, DateTime date)
        {
            var books = context.Books
                .Where(b=>b.PublishedOn < date && b.Genre == Genre.Science)
                .Select(b=> new BookExportDto
                {
                    Name = b.Name,
                    Date = b.PublishedOn.ToString("d", CultureInfo.InvariantCulture),
                    Pages = b.Pages
                })
                .ToArray()
                .OrderByDescending(x=>x.Pages)
                .ThenByDescending(x=>x.Date)
                .Take(10)
                .ToArray();
            XmlRootAttribute root = new XmlRootAttribute("Books");
            XmlSerializer serializer = new XmlSerializer(typeof(BookExportDto[]), root);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            StringBuilder sb = new StringBuilder();
            using(var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, books,ns);
            }
            return sb.ToString().TrimEnd();
        }
    }
}