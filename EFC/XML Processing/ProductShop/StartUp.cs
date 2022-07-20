using ProductShop.Data;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            ProductShopContext db = new ProductShopContext();

            //ex 1
            var xml = File.ReadAllText(@".\Datasets\users.xml");
            Console.WriteLine(ImportUsers(db, xml));

            //ex 2
        }
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute attribute = new XmlRootAttribute("Users");
            XmlSerializer serializer = new XmlSerializer(typeof(UserInputDto[]), attribute);
            using StringReader reader = new StringReader(inputXml);
            var usersDto = serializer.Deserialize(reader) as UserInputDto[];
            var users = usersDto.Select(x => new User
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Age = x.Age
            })
            .ToList();
            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Count()}.";
        }
    }
}