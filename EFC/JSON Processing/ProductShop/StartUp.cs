using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Dtos.Input;
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
            var inputJson = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\JSON Processing\ProductShop\Datasets\products.json");
            Console.WriteLine(ImportProducts(context, inputJson));
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
    }
}