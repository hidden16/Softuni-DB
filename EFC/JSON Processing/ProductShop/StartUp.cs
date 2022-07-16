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
            var inputJson = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\JSON Processing\ProductShop\Datasets\users.json");
            Console.WriteLine(ImportUsers(context, inputJson));
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
    }
}