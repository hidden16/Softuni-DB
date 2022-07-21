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
            var inputXml = File.ReadAllText(@"D:\Git\Softuni-DB\EFC\XML Processing\CarDealer\Datasets\suppliers.xml");
            Console.WriteLine(ImportSuppliers(db, inputXml));
        }
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute attribute = new XmlRootAttribute("Suppliers");
            XmlSerializer serializer = new XmlSerializer(typeof(SuppliersImportDto[]), attribute);
            var reader = new StringReader(inputXml);
            var suppliersDto = serializer.Deserialize(reader) as SuppliersImportDto[];
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
    }
}