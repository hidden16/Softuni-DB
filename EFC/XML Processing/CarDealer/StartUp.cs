using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using System;
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
            var inputXml = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\XML Processing\CarDealer\Datasets\parts.xml");
            Console.WriteLine(ImportParts(db, inputXml));
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
    }
}