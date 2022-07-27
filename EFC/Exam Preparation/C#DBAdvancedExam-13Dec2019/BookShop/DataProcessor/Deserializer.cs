namespace BookShop.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using AutoMapper;
    using BookShop.Data.Models;
    using BookShop.Data.Models.Enums;
    using BookShop.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;
    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedBook
            = "Successfully imported book {0} for {1:F2}.";

        private const string SuccessfullyImportedAuthor
            = "Successfully imported author - {0} with {1} books.";

        public static string ImportBooks(BookShopContext context, string xmlString)
        {
            XmlRootAttribute attribute = new XmlRootAttribute("Books");
            XmlSerializer serializer = new XmlSerializer(typeof(BookImportDto[]), attribute);
            var reader = new StringReader(xmlString);
            var booksDto = serializer.Deserialize(reader) as BookImportDto[];

            List<Book> books = new List<Book>();
            StringBuilder sb = new StringBuilder();
            foreach (BookImportDto book in booksDto)
            {
                if (!IsValid(book))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                books.Add(new Book()
                {
                    Name = book.Name,
                    Genre = (Genre)book.Genre,
                    Pages = book.Pages,
                    Price = book.Price,
                    PublishedOn = DateTime.ParseExact(book.PublishedOn, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                });
                sb.AppendLine(String.Format(SuccessfullyImportedBook, book.Name, book.Price));
            }
            context.AddRange(books);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportAuthors(BookShopContext context, string jsonString)
        {
            var authorsJson = JsonConvert.DeserializeObject<AuthorImportDto[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Author> authors = new List<Author>();
            foreach (var author in authorsJson)
            {
                if (!IsValid(author))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                bool emailExist = authors.Any(x => x.Email == author.Email);
                if (emailExist)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var authorToAdd = new Author()
                {
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    Phone = author.Phone,
                    Email = author.Email,
                };
                foreach (var book in author.Books)
                {
                    if (context.Books.Any(x => x.Id == book.Id))
                    {
                        authorToAdd.AuthorsBooks.Add(new AuthorBook
                        {
                            Author = authorToAdd,
                            BookId = (int)book.Id,
                        });
                    }
                }
                if (authorToAdd.AuthorsBooks.Count() == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                sb.AppendLine(string.Format(SuccessfullyImportedAuthor, $"{author.FirstName} {author.LastName}", authorToAdd.AuthorsBooks.Count));
                authors.Add(authorToAdd);
            }
            context.Authors.AddRange(authors);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}