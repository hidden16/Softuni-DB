namespace VaporStore.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var genresDto = context.Genres
                .Where(x => genreNames.Any(gn => gn == x.Name) && x.Games.Any(x => x.Purchases.Any()))
                .ToList()
                .Select(x => new GenreExport()
                {
                    Id = x.Id,
                    Genre = x.Name,
                    Games = x.Games
                    .Where(g => g.Purchases.Any())
                    .Select(g => new GenreGameExport()
                    {
                        Id = g.Id,
                        Tags = String.Join(", ", g.GameTags.Select(t => t.Tag.Name)),
                        Title = g.Name,
                        Players = g.Purchases.Count,
                        Developer = g.Developer.Name
                    })
                    .OrderByDescending(x => x.Players)
                    .ThenBy(x => x.Id)
                    .ToArray(),
                    TotalPlayers = x.Games.Sum(x => x.Purchases.Count)
                })
                .OrderByDescending(x => x.TotalPlayers)
                .ThenBy(x => x.Id)
                .ToList();
            return JsonConvert.SerializeObject(genresDto);
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            var enumType = (PurchaseType)Enum.Parse(typeof(PurchaseType), storeType);
            var userDto = context.Users
                .Where(x => x.Cards.Any(x => x.Purchases.Any()))
                .ToArray()
                .Select(u => new UserExport
                {
                    Username = u.Username,
                    Purchases = context
                    .Purchases
                    .Where(c => c.Card.User.Username == u.Username && c.Type == enumType)
                    .OrderBy(x=>x.Date)
                    .ToArray()
                    .Select(p => new UserPurchaseExport
                    {
                        Cvc = p.Card.Cvc,
                        Card = p.Card.Number,
                        Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        Game = new UserPurchaseGameExport
                        {
                            Genre = p.Game.Genre.Name,
                            Price = p.Game.Price,
                            Title = p.Game.Name
                        }
                    })
                    .ToArray(),
                    TotalSpent = context.Purchases
                    .Where(x=>x.Card.User.Username == u.Username && x.Type == enumType)
                    .Sum(x=>x.Game.Price)
                })
                .Where(x=>x.Purchases.Length > 0)
                .OrderByDescending(x=>x.TotalSpent)
                .ThenBy(x=>x.Username)
                .ToArray();
            XmlRootAttribute rootAttribute = new XmlRootAttribute("Users");
            XmlSerializer serializer = new XmlSerializer(typeof(UserExport[]), rootAttribute);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            StringBuilder sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, userDto, ns);
            }
            return sb.ToString().TrimEnd();
        }
    }
}