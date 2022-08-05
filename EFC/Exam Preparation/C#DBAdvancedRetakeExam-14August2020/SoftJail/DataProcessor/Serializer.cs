namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisonersDto = context.Prisoners
                .Where(x => ids.Contains(x.Id))
                .ToArray()
                .Select(x => new PrisonerExport()
                {
                    Id = x.Id,
                    Name = x.FullName,
                    CellNumber = x.Cell.CellNumber,
                    Officers = x.PrisonerOfficers
                    .Select(po => new OfficerExport()
                    {
                        OfficerName = po.Officer.FullName,
                        Department = po.Officer.Department.Name
                    })
                    .OrderBy(x => x.OfficerName)
                    .ToArray(),
                    TotalOfficerSalary = Math.Round(x.PrisonerOfficers.Sum(x => x.Officer.Salary), 2)
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToArray();
            return JsonConvert.SerializeObject(prisonersDto);
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var prisonersNamesSplitted = prisonersNames.Split(",");
            var prisonersDto = context.Prisoners
                .Where(x => prisonersNamesSplitted.Contains(x.FullName))
                .ToArray()
                .Select(x => new InboxPrisonerExport
                {
                    Id = x.Id,
                    Name = x.FullName,
                    IncarcerationDate = x.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = x.Mails
                    .Select(m => new EncryptedMailExport
                    {
                        Description = String.Join("", m.Description.Reverse())
                    })
                    .ToArray()
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToArray();
            XmlRootAttribute rootAttribute = new XmlRootAttribute("Prisoners");
            XmlSerializer serializer = new XmlSerializer(typeof(InboxPrisonerExport[]), rootAttribute);
            StringBuilder sb = new StringBuilder();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, prisonersDto, ns);
            }
            return sb.ToString().TrimEnd();
        }
    }
}