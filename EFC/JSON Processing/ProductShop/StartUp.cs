using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Dtos.Input;
using ProductShop.Dtos.Output;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            ProductShopContext context = new ProductShopContext();
            // ex 1
            //var inputJson = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\JSON Processing\ProductShop\Datasets\users.json");
            //Console.WriteLine(ImportUsers(context, inputJson));

            //ex2
            //var inputJson = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\JSON Processing\ProductShop\Datasets\products.json");
            //Console.WriteLine(ImportProducts(context, inputJson));  

            //ex 3
            //var inputJson = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\JSON Processing\ProductShop\Datasets\categories.json");
            //Console.WriteLine(ImportCategories(context, inputJson));

            //ex 4
            //var inputJson = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\JSON Processing\ProductShop\Datasets\categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, inputJson));

            //ex 5
            Console.WriteLine(GetProductsInRange(context));
        }
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var usersInput = JsonConvert.DeserializeObject<UserInputDto[]>(inputJson);
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            IMapper mapper = new Mapper(mapperConfig);
            var mappedUsers = mapper.Map<User[]>(usersInput);
            context.Users.AddRange(mappedUsers);
            context.SaveChanges();
            return $"Successfully imported {usersInput.Length}";
        }
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            // SOLUTION WITHOUT MAPPING
            //var products = JsonConvert.DeserializeObject<Product[]>(inputJson);
            //context.Products.AddRange(products);
            //context.SaveChanges();
            //-----------------------------------------------

            // SOLUTION WITH MAPPING
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            IMapper map = new Mapper(mapConfig);
            var productMap = map.Map<Product[]>(products);
            context.Products.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Length}";
        }
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<CategoryInputDto[]>(inputJson)
                .Where(x=> !string.IsNullOrEmpty(x.Name));
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            IMapper mapper = new Mapper(mapperConfig);
            var mappedCategories = mapper.Map<Category[]>(categories);
            context.Categories.AddRange(mappedCategories);
            context.SaveChanges();
            return $"Successfully imported {categories.Count()}";
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoryProducts = JsonConvert.DeserializeObject<CategoryProductDto[]>(inputJson);
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            IMapper mapper = new Mapper(mapConfig);
            var categoryProductsMapper = mapper.Map<CategoryProduct[]>(categoryProducts);
            context.CategoryProducts.AddRange(categoryProductsMapper);
            context.SaveChanges();
            return $"Successfully imported {categoryProducts.Length}";
        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            var productsInRange = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x=>x.Price)
                .Select(x => new ProductOutputDto
                {
                    name = x.Name,
                    price = x.Price,
                    seller = $"{x.Seller.FirstName} {x.Seller.LastName}"
                });

            var jsonProducts = JsonConvert.SerializeObject(productsInRange);
            File.WriteAllText(@"D:\Git\Softuni-DB\EFC\JSON Processing\ProductShop\Datasets\products-in-range.json", jsonProducts);
            return jsonProducts;
        }
    }
}