using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using ProductShop.XmlAssistance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
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
            //var xml = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\XML Processing\ProductShop\Datasets\products.xml");
            //Console.WriteLine(ImportProducts(db, xml));

            //ex 3
            //var xml = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\XML Processing\ProductShop\Datasets\categories.xml");
            //Console.WriteLine(ImportCategories(db,xml));

            //ex 4
            //var xml = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\XML Processing\ProductShop\Datasets\categories-products.xml");
            //Console.WriteLine(ImportCategoryProducts(db, xml));

            //ex 5
            //Console.WriteLine(GetProductsInRange(db));

            //ex 6
            //Console.WriteLine(GetSoldProducts(db));

            //ex 7
            Console.WriteLine(GetCategoriesByProductsCount(db));
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
            var productsDto = serializer.Deserialize(reader) as ProductInputDto[];
            var products = productsDto
                .Select(x => new Product
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
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute attribute = new XmlRootAttribute("Categories");
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryInputDto[]), attribute);
            using StringReader reader = new StringReader(inputXml);
            var categoriesDto = serializer.Deserialize(reader) as CategoryInputDto[];
            var categories = categoriesDto
                .Where(x => x.Name != null)
                .Select(x => new Category
                {
                    Name = x.Name
                })
                .ToList();
            context.Categories.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Count}";
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlRootAttribute attribute = new XmlRootAttribute("CategoryProducts");
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryProductInputDto[]), attribute);
            using StringReader reader = new StringReader(inputXml);
            var categoryProductsDto = serializer.Deserialize(reader) as CategoryProductInputDto[];
            var categoryProducts = categoryProductsDto
                .Where(x => context.Products.Any(y => y.Id == x.ProductId)
                       && context.Categories.Any(y => y.Id == x.CategoryId))
                .Select(x => new CategoryProduct
                {
                    ProductId = x.ProductId,
                    CategoryId = x.CategoryId
                })
                .ToList();
            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();
            return $"Successfully imported {categoryProducts.Count}";
        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Take(10)
                .Select(x => new ProductsOutputDto
                {
                    name = x.Name,
                    price = x.Price,
                    buyer = $"{x.Buyer.FirstName} {x.Buyer.LastName}"
                })
                .ToArray();
            return XAssist.Serialize(products, "Products");
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Count() > 0)
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new UserSoldProductOutputDto
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold.Select(x => new SoldProductDto
                    {
                        Name = x.Name,
                        Price = x.Price
                    })
                    .ToArray()
                })
                .Take(5)
                .ToArray();
            return XAssist.Serialize(users, "Users");
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(x => new CategoryByProductCountDto
                {
                    Name = x.Name,
                    Count = x.CategoryProducts.Count(),
                    AveragePrice = x.CategoryProducts.Average(x => x.Product.Price),
                    TotalRevenue = x.CategoryProducts.Sum(x => x.Product.Price)
                })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.TotalRevenue)
                .ToList();
            return XAssist.Serialize(categories, "Categories");
        }
    }
}