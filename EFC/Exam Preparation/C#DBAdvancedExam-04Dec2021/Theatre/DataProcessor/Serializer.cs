namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatresDto = context.Theatres
                //.ToArray() => For Judge !!!
                .Where(x => x.NumberOfHalls >= numbersOfHalls && x.Tickets.Count >= 20)
                .Select(t=> new TheatreExport
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets.Where(x=>x.RowNumber >= 1 && x.RowNumber <= 5).Sum(x=>x.Price),
                    Tickets = t.Tickets
                    .Where(x=>x.RowNumber >= 1 && x.RowNumber <= 5)
                    .Select(x=> new TheatreTicketExport
                    {
                        Price = x.Price,
                        RowNumber = x.RowNumber
                    })
                    .OrderByDescending(x=>x.Price)
                    .ToArray()
                })
                .OrderByDescending(x=>x.Halls)
                .ThenBy(x=>x.Name)
                .ToArray();
            var serialized = JsonConvert.SerializeObject(theatresDto);
            return serialized;
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            var playsDto = context.Plays
                //.ToArray() => For Judge !!!
                .Where(x => x.Rating <= rating)
                .Select(p => new PlayExport
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c", CultureInfo.InvariantCulture),
                    Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                    Genre = p.Genre,
                    Actors = p.Casts
                    .Where(x => x.IsMainCharacter == true)
                    .Select(x => new PlayCastExport
                    {
                        FullName = x.FullName,
                        MainCharacter = $"Plays main character in '{p.Title}'."
                    })
                    .OrderByDescending(x=>x.FullName)
                    .ToArray()
                })
                .OrderBy(x=>x.Title)
                .ThenByDescending(x=>x.Genre)
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Plays");
            XmlSerializer serializer = new XmlSerializer(typeof(PlayExport[]), root);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            StringBuilder sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, playsDto, ns);
            }
            return sb.ToString().TrimEnd();
        }
    }
}
