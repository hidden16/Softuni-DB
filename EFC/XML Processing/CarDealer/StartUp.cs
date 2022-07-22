using CarDealer.Data;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using CarDealer.XmlAssistance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext db = new CarDealerContext();

            //ex 9
            //var inputXml = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\XML Processing\CarDealer\Datasets\suppliers.xml");
            //Console.WriteLine(ImportSuppliers(db, inputXml));

            //ex 10
            //var inputXml = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\XML Processing\CarDealer\Datasets\parts.xml");
            //Console.WriteLine(ImportParts(db, inputXml));

            //ex 11
            //var inputXml = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\XML Processing\CarDealer\Datasets\cars.xml");
            //Console.WriteLine(ImportCars(db, inputXml));

            //ex 12
            //var inputXml = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\XML Processing\CarDealer\Datasets\customers.xml");
            //Console.WriteLine(ImportCustomers(db, inputXml));

            //ex 13
            //var inputXml = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\XML Processing\CarDealer\Datasets\sales.xml");
            //Console.WriteLine(ImportSales(db, inputXml));

            //ex 14
            //Console.WriteLine(GetCarsWithDistance(db));

            //ex 15
            //Console.WriteLine(GetCarsFromMakeBmw(db));

            //ex 16
            //Console.WriteLine(GetLocalSuppliers(db));

            //ex 17
            //Console.WriteLine(GetCarsWithTheirListOfParts(db));

            //ex 18
            Console.WriteLine(GetTotalSalesByCustomer(db));
        }
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute attribute = new XmlRootAttribute("Suppliers");
            XmlSerializer serializer = new XmlSerializer(typeof(SupplierImportDto[]), attribute);
            var reader = new StringReader(inputXml);
            var suppliersDto = serializer.Deserialize(reader) as SupplierImportDto[];
            var suppliers = suppliersDto.Select(x => new Supplier
            {
                Name = x.Name,
                IsImporter = x.IsImporter
            })
                .ToList();
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Count}";
        }
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute attribute = new XmlRootAttribute("Parts");
            XmlSerializer serializer = new XmlSerializer(typeof(PartImportDto[]), attribute);
            var reader = new StringReader(inputXml);
            var partsDto = serializer.Deserialize(reader) as PartImportDto[];
            var parts = partsDto
                .Where(x => context.Suppliers.Any(s => s.Id == x.SupplierId))
                .Select(x => new Part
                {
                    Name = x.Name,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    SupplierId = x.SupplierId
                })
                .ToList();
            context.Parts.AddRange(parts);
            context.SaveChanges();
            return $"Successfully imported {parts.Count}";
        }
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var carsDto = XAssist.Deserialize<CarsImportDto>("Cars", inputXml);
            var cars = new List<Car>();
            foreach (var car in carsDto)
            {
                var partsId = car.Parts
                    .Select(x => x.PartId)
                    .Distinct()
                    .Where(id => context.Parts.Any(x => x.Id == id))
                    .ToArray();

                var currCar = new Car
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance,
                    PartCars = partsId.Select(x => new PartCar()
                    {
                        PartId = x
                    })
                    .ToArray()
                };

                cars.Add(currCar);
            }
            context.Cars.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {cars.Count}";
        }
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var customersDto = XAssist.Deserialize<CustomerImportDto>("Customers", inputXml);
            var customers = customersDto
                .Select(x => new Customer()
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate,
                    IsYoungDriver = x.IsYoungDriver,
                })
                .ToList();
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count}";
        }
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var salesDto = XAssist.Deserialize<SalesImportDto>("Sales", inputXml);
            var carIds = context.Cars.Select(x => x.Id).ToList();
            var sales = salesDto
                .Where(x => carIds.Contains(x.CarId))
                .Select(x => new Sale()
                {
                    CarId = x.CarId,
                    CustomerId = x.CustomerId,
                    Discount = x.Discount,
                })
                .ToList();
            context.Sales.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Count}";
        }
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var carsDto = context.Cars
                .Where(x => x.TravelledDistance > 2_000_000)
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Select(x => new CarDistanceExportDto
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .Take(10)
                .ToList();
            return XAssist.Serialize(carsDto, "cars");
        }
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var bmwCars = context.Cars
                .Where(x => x.Make == "BMW")
                .Select(x => new CarFromBmwExportDto
                {
                    Id = x.Id,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToList();
            return XAssist.Serialize(bmwCars, "cars");
        }
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(x => !x.IsImporter)
                .Select(x => new LocalSupplierExportDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count()
                })
                .ToList();
            return XAssist.Serialize(suppliers, "suppliers");
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carParts = context.Cars
                .Select(x => new CarPartExportDto
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance,
                    Parts = x.PartCars.Select(c => new PartExportDto
                    {
                        Name = c.Part.Name,
                        Price = c.Part.Price
                    })
                    .OrderByDescending(x => x.Price)
                    .ToArray()
                })
                .OrderByDescending(x => x.TravelledDistance)
                .ThenBy(x => x.Model)
                .Take(5)
                .ToList();
            return XAssist.Serialize(carParts, "cars");
        }
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(x => x.Sales.Any())
                .Select(x => new CustomerSaleExportDto
                {
                    FullName = x.Name,
                    BoughtCars = x.Sales.Count,
                    SpentMoney = x.Sales.Sum(x => x.Car.PartCars.Sum(x=>x.Part.Price))
                })
                .OrderByDescending(x => x.SpentMoney)
                .ToList();
            return XAssist.Serialize(customers, "customers");
        }
    }
}