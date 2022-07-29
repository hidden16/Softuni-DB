
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context.Shells
                .Where(x => x.ShellWeight > shellWeight)
                .Select(x => new ShellExport
                {
                    ShellWeight = x.ShellWeight,
                    Caliber = x.Caliber,
                    Guns = x.Guns
                    .Where(x => x.GunType == GunType.AntiAircraftGun)
                    .Select(x => new ShellGunExport
                    {
                        BarrelLength = x.BarrelLength,
                        GunType = x.GunType.ToString(),
                        GunWeight = x.GunWeight,
                        Range = x.Range > 3000 ? "Long-range" : "Regular range"
                    })
                    .OrderByDescending(x => x.GunWeight)
                    .ToArray()
                })
                .OrderBy(x => x.ShellWeight)
                .ToList();
            return JsonConvert.SerializeObject(shells, Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            var guns = context.Guns
                .Where(x => x.Manufacturer.ManufacturerName == manufacturer)
                .Select(x => new GunExport
                {
                    Manufacturer = x.Manufacturer.ManufacturerName,
                    BarrelLength = x.BarrelLength,
                    GunType = x.GunType.ToString(),
                    GunWeight = x.GunWeight,
                    Range = x.Range,
                    Countries = x.CountriesGuns
                    .Where(x => x.Country.ArmySize > 4_500_000)
                    .Select(x => new GunCountryExport
                    {
                        ArmySize = x.Country.ArmySize,
                        Country = x.Country.CountryName
                    })
                    .OrderBy(x => x.ArmySize)
                    .ToArray()
                })
                .ToArray()
                .OrderBy(x => x.BarrelLength)
                .ToArray();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer serializer = new XmlSerializer(typeof(GunExport[]), new XmlRootAttribute("Guns"));
            StringBuilder sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, guns, ns);
            }
            return sb.ToString().TrimEnd();
        }
    }
}
