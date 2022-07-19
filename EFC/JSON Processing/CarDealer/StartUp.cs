using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO.Input;
using CarDealer.Models;
using Newtonsoft.Json;
namespace CarDealer
{
    public class StartUp
    {
        private static IMapper mapper;
        public static void Main(string[] args)
        {
            CarDealerContext db = new CarDealerContext();
            //db.Database.EnsureCreated();
            //db.Database.EnsureDeleted();

            //ex 9
            //var inputJson = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\JSON Processing\CarDealer\Datasets\suppliers.json");
            //Console.WriteLine(ImportSuppliers(db, inputJson));

            //ex 10
            //var inputJosn = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\JSON Processing\CarDealer\Datasets\parts.json");
            //Console.WriteLine(ImportParts(db, inputJosn));

            //ex 11
            //var inputsJson = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\JSON Processing\CarDealer\Datasets\cars.json");
            //Console.WriteLine(ImportCars(db, inputsJson));

            //ex 12
            //var inputJson = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\JSON Processing\CarDealer\Datasets\customers.json");
            //Console.WriteLine(ImportCustomers(db, inputJson));

            //ex 13
            //var inputJsons = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\JSON Processing\CarDealer\Datasets\sales.json");
            //Console.WriteLine(ImportSales(db,inputJsons));

            //ex 14
            Console.WriteLine(GetOrderedCustomers(db));
        }
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var suppliers = JsonConvert.DeserializeObject<SupplierDto[]>(inputJson);
            var suppliersMapped = mapper.Map<Supplier[]>(suppliers);
            context.Suppliers.AddRange(suppliersMapped);
            context.SaveChanges();
            return $"Successfully imported {suppliersMapped.Count()}.";
        }
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var suppliersCount = context.Suppliers.Count();
            var parts = JsonConvert.DeserializeObject<PartDto[]>(inputJson)
                .Where(x => x.SupplierId <= suppliersCount)
                .ToArray();
            var partsMapped = mapper.Map<Part[]>(parts);
            context.Parts.AddRange(partsMapped);
            context.SaveChanges();
            return $"Successfully imported {partsMapped.Count()}.";
        }
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsJson = JsonConvert.DeserializeObject<List<CarDto>>(inputJson);
            var carsToAdd = new List<Car>();
            foreach (var cars in carsJson)
            {
                var car = new Car()
                {
                    Make = cars.Make,
                    Model = cars.Model,
                    TravelledDistance = cars.TravelledDistance
                };

                foreach (var partId in cars.PartsId.Distinct())
                {
                    car.PartCars.Add(new PartCar()
                    {
                        PartId = partId
                    });
                }
                carsToAdd.Add(car);
            }
            context.Cars.AddRange(carsToAdd);
            context.SaveChanges();
            return $"Successfully imported {carsToAdd.Count()}.";
        }
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var customers = JsonConvert.DeserializeObject<CustomerDto[]>(inputJson);
            var customersToAdd = mapper.Map<Customer[]>(customers);
            context.Customers.AddRange(customersToAdd);
            context.SaveChanges();
            return $"Successfully imported {customersToAdd.Count()}."; ;
        }
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            InitializeMapper();
            var salesFromJson = JsonConvert.DeserializeObject<SaleDto[]>(inputJson);
            var sales = mapper.Map<Sale[]>(salesFromJson).ToList();
            context.Sales.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Count()}.";
        }
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(x => new 
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = x.IsYoungDriver
                })
                .ToList();

            var customersJson = JsonConvert.SerializeObject(customers);
            File.WriteAllText(@"D:\Git\Softuni-DB\EFC\JSON Processing\CarDealer\Datasets\orderedCustomers.json", customersJson);
            return customersJson;
        }
        private static void InitializeMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            mapper = config.CreateMapper();
        }
    }
}