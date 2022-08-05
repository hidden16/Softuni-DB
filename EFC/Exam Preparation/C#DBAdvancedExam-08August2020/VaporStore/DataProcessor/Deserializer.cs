namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
    {
        private const string errorMessage = "Invalid Data";
        private const string successfullyAddedGames = "Added {0} ({1}) with {2} tags";
        private const string successfullyAddedUsers = "Imported {0} with {1} cards";
        private const string successfullyAddedPurchases = "Imported {0} for {1}";
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gamesDto = JsonConvert.DeserializeObject<GameExport[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Game> games = new List<Game>();
            List<Developer> devs = new List<Developer>();
            List<Genre> genres = new List<Genre>();
            List<Tag> tags = new List<Tag>();
            foreach (var game in gamesDto)
            {
                if (!IsValid(game))
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }
                var isDateValid = DateTime.TryParseExact(game.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
                if (!isDateValid)
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }
                if (game.Tags.Count() == 0)
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }
                Game gameToAdd = new Game()
                {
                    ReleaseDate = DateTime.ParseExact(game.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Name = game.Name,
                    Price = game.Price
                };

                var dev = devs.FirstOrDefault(x => x.Name == game.Developer);
                if (dev == null)
                {
                    Developer developer = new Developer()
                    {
                        Name = game.Developer,
                    };
                    devs.Add(developer);
                    gameToAdd.Developer = developer;
                }
                else
                {
                    gameToAdd.Developer = dev;
                }

                var genre = genres.FirstOrDefault(x => x.Name == game.Genre);
                if (genre == null)
                {
                    Genre g = new Genre()
                    {
                        Name = game.Genre,
                    };
                    genres.Add(g);
                    gameToAdd.Genre = g;
                }
                else
                {
                    gameToAdd.Genre = genre;
                }

                foreach (var tag in game.Tags)
                {
                    if (string.IsNullOrEmpty(tag))
                    {
                        continue;
                    }
                    Tag t = tags.FirstOrDefault(x => x.Name == tag);
                    if (t == null)
                    {
                        Tag newTag = new Tag()
                        {
                            Name = tag
                        };
                        tags.Add(newTag);
                        gameToAdd.GameTags.Add(new GameTag()
                        {
                            Game = gameToAdd,
                            Tag = newTag
                        });
                    }
                    else
                    {
                        gameToAdd.GameTags.Add(new GameTag()
                        {
                            Game = gameToAdd,
                            Tag = t
                        });
                    }
                }
                if (gameToAdd.GameTags.Count == 0)
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }
                sb.AppendLine(string.Format(successfullyAddedGames, game.Name, game.Genre, gameToAdd.GameTags.Count));
                games.Add(gameToAdd);
            }
            context.Games.AddRange(games);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var usersDto = JsonConvert.DeserializeObject<UserImport[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<User> users = new List<User>();
            foreach (var user in usersDto)
            {
                if (!IsValid(user))
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }

                if (user.Cards.Count() == 0)
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }

                var cardTypes = new string[]
                {
                    CardType.Credit.ToString(),
                    CardType.Debit.ToString()
                };
                User userToAdd = new User()
                {
                    Age = user.Age,
                    Email = user.Email,
                    FullName = user.FullName,
                    Username = user.Username
                };
                foreach (var card in user.Cards)
                {
                    if (!IsValid(card))
                    {
                        sb.AppendLine(errorMessage);
                        continue;
                    }
                    if (!cardTypes.Any(x => x == card.Type))
                    {
                        sb.AppendLine(errorMessage);
                        continue;
                    }
                    userToAdd.Cards.Add(new Card()
                    {
                        Cvc = card.Cvc,
                        Type = (CardType)Enum.Parse(typeof(CardType), card.Type),
                        Number = card.Number
                    });
                }
                sb.AppendLine(string.Format(successfullyAddedUsers, user.Username, userToAdd.Cards.Count));
                users.Add(userToAdd);
            }
            context.Users.AddRange(users);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            XmlRootAttribute rootAttribute = new XmlRootAttribute("Purchases");
            XmlSerializer serializer = new XmlSerializer(typeof(PurchaseExport[]), rootAttribute);
            var reader = new StringReader(xmlString);
            var purchasesDto = serializer.Deserialize(reader) as PurchaseExport[];
            List<Purchase> purchases = new List<Purchase>();
            StringBuilder sb = new StringBuilder();
            foreach (var purchase in purchasesDto)
            {
                if (!IsValid(purchase))
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }
                var isDateValid = DateTime.TryParseExact(purchase.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
                if (!isDateValid)
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }
                Card card = context.Cards
                    .FirstOrDefault(x=>x.Number == purchase.Card);
                if (card == null)
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }
                Game game = context.Games
                    .FirstOrDefault(x => x.Name == purchase.Title);
                if (game == null)
                {
                    sb.AppendLine(errorMessage);
                    continue;
                }
                Purchase purchaseToAdd = new Purchase()
                {
                    Type = (PurchaseType)Enum.Parse(typeof(PurchaseType), purchase.Type),
                    ProductKey = purchase.Key,
                    Game = game,
                    Card = card,
                    Date = DateTime.ParseExact(purchase.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
                };
                sb.AppendLine(string.Format(successfullyAddedPurchases, purchase.Title, card.User.Username));
                purchases.Add(purchaseToAdd);
            }
            context.Purchases.AddRange(purchases);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}