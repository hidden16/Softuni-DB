namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage =
                "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            XmlRootAttribute root = new XmlRootAttribute("Countries");
            XmlSerializer serializer = new XmlSerializer(typeof(CountryImport[]), root);
            StringReader reader = new StringReader(xmlString);
            var countriesDto = serializer.Deserialize(reader) as CountryImport[];
            StringBuilder sb = new StringBuilder();
            List<Country> countries = new List<Country>();
            foreach (var countryDto in countriesDto)
            {
                if (!IsValid(countryDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                sb.AppendLine(string.Format(SuccessfulImportCountry, countryDto.CountryName, countryDto.ArmySize));
                countries.Add(new Country
                {
                    CountryName = countryDto.CountryName,
                    ArmySize = countryDto.ArmySize
                });
            }
            context.Countries.AddRange(countries);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            var manufacturersDto = XmlDeserializerAssist<ManufacturerImport>("Manufacturers", xmlString);
            StringBuilder sb = new StringBuilder();
            var manufacturers = new List<Manufacturer>();
            foreach (var manufacturer in manufacturersDto)
            {
                if (!IsValid(manufacturer))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var foundedSplit = manufacturer.Founded.Split(", ");
                if (manufacturers.Any(x => x.ManufacturerName == manufacturer.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                sb.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, $"{foundedSplit[foundedSplit.Length - 2]}, {foundedSplit[foundedSplit.Length - 1]}"));
                manufacturers.Add(new Manufacturer
                {
                    ManufacturerName = manufacturer.ManufacturerName,
                    Founded = manufacturer.Founded
                });
            }
            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            var shellsDto = XmlDeserializerAssist<ShellImport>("Shells", xmlString);
            var shells = new List<Shell>();
            StringBuilder sb = new StringBuilder();
            foreach (var shell in shellsDto)
            {
                if (!IsValid(shell))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                sb.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
                shells.Add(new Shell
                {
                    ShellWeight = shell.ShellWeight,
                    Caliber = shell.Caliber
                });
            }
            context.Shells.AddRange(shells);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            var gunsDto = JsonDeserializerAssist<GunImport>(jsonString);
            StringBuilder sb = new StringBuilder();
            var guns = new List<Gun>();
            foreach (var gun in gunsDto)
            {
                if (!IsValid(gun))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var gunTypeArray = new string[]
                {
                    GunType.Mortar.ToString(),
                    GunType.Howitzer.ToString(),
                    GunType.Mortar.ToString(),
                    GunType.FieldGun.ToString(),
                    GunType.AntiAircraftGun.ToString(),
                    GunType.MountainGun.ToString(),
                    GunType.AntiTankGun.ToString()
                };
                if (!gunTypeArray.Any(x => x == gun.GunType))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                List<CountryGun> countryGun = new List<CountryGun>();
                foreach (var countryId in gun.Countries)
                {
                    countryGun.Add(new CountryGun()
                    {
                        CountryId = countryId.Id
                    });
                }
                Gun gunToAdd = new Gun()
                {
                    BarrelLength = gun.BarrelLength,
                    GunType = (GunType)Enum.Parse(typeof(GunType), gun.GunType),
                    GunWeight = gun.GunWeight,
                    ManufacturerId = gun.ManufacturerId,
                    NumberBuild = gun.NumberBuild,
                    Range = gun.Range,
                    ShellId = gun.ShellId,
                    CountriesGuns = countryGun
                };
                sb.AppendLine(string.Format(SuccessfulImportGun, gun.GunType, gun.GunWeight, gun.BarrelLength));
                guns.Add(gunToAdd);
            }
            context.Guns.AddRange(guns);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
        private static T[] XmlDeserializerAssist<T>(string rootAttribute, string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T[]), new XmlRootAttribute(rootAttribute));
            var reader = new StringReader(xmlString);
            var deserializedObject = serializer.Deserialize(reader) as T[];
            return deserializedObject;
        }
        private static T[] JsonDeserializerAssist<T>(string jsonString)
        {
            var deserializedObject = JsonConvert.DeserializeObject<T[]>(jsonString);
            return deserializedObject;
        }

    }
}
