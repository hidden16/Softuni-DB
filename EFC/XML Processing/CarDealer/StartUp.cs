using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using CarDealer.XmlAssistance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var inputXml = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\XML Processing\CarDealer\Datasets\customers.xml");
            Console.WriteLine(ImportCustomers(db,inputXml));
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
                    .Where(id => context.Parts.Any(x=>x.Id == id))
                    .ToArray();

                var currCar = new Car
                {
                    Make=car.Make,
                    Model=car.Model,
                    TravelledDistance=car.TravelledDistance,
                    PartCars = partsId.Select(x=> new PartCar()
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
    }
}