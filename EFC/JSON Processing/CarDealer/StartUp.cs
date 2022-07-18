using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO.Input;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext db = new CarDealerContext();
            //db.Database.EnsureCreated();
            //db.Database.EnsureDeleted();

            //ex 9
            //var inputJson = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\JSON Processing\CarDealer\Datasets\suppliers.json");
            //Console.WriteLine(ImportSuppliers(db, inputJson));

            //ex 10
            var inputJosn = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\JSON Processing\CarDealer\Datasets\parts.json");
            Console.WriteLine(ImportParts(db, inputJosn));
        }
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<SupplierDto[]>(inputJson);
            var supplierConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            IMapper mapper = new Mapper(supplierConfig);
            var suppliersMapped = mapper.Map<Supplier[]>(suppliers);
            context.Suppliers.AddRange(suppliersMapped);
            context.SaveChanges();
            return $"Successfully imported {suppliersMapped.Count()}.";
        }
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var suppliersCount = context.Suppliers.Count();
            var parts = JsonConvert.DeserializeObject<PartDto[]>(inputJson)
                .Where(x=>x.SupplierId <= suppliersCount)
                .ToArray();
            var partsConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            IMapper mapper = new Mapper(partsConfig);
            var partsMapped = mapper.Map<Part[]>(parts);
            context.Parts.AddRange(partsMapped);
            context.SaveChanges();
            return $"Successfully imported {partsMapped.Count()}.";
        }
    }
}