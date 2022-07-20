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
            //var xml = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\XML Processing\ProductShop\Datasets\users.xml");
            //Console.WriteLine(ImportUsers(db, xml));

            //ex 2
            var xml = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\XML Processing\ProductShop\Datasets\products.xml");
            Console.WriteLine(ImportProducts(db,xml));
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
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute attribute = new XmlRootAttribute("Products");
            XmlSerializer serializer = new XmlSerializer(typeof(ProductInputDto[]), attribute);
            using StringReader reader = new StringReader(inputXml);
            var productsDto = serializer.Deserialize(reader)as ProductInputDto[];
            var products = productsDto.Select(x => new Product
            {
                Name = x.Name,
                Price = x.Price,
                SellerId = x.SellerId,
                BuyerId = x.BuyerId
            })
                .ToList();
            context.Products.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Count()}";
        }
    }
}