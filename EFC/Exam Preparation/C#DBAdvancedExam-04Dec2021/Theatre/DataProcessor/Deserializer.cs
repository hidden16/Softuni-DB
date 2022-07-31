namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            var playsDto = XmlDeserializer<PlayImport>("Plays", xmlString);
            StringBuilder sb = new StringBuilder();
            List<Play> plays = new List<Play>();
            foreach (var play in playsDto)
            {
                if (!IsValid(play))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var genreCheck = new Genre[]
                {
                    Genre.Comedy,
                    Genre.Drama,
                    Genre.Musical,
                    Genre.Romance
                };
                if (!genreCheck.Any(x => x.ToString() == play.Genre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                if (TimeSpan.Parse(play.Duration).Hours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre, play.Rating));
                plays.Add(new Play()
                {
                    Genre = (Genre)Enum.Parse(typeof(Genre), play.Genre),
                    Description = play.Description,
                    Duration = TimeSpan.ParseExact(play.Duration, "c", CultureInfo.InvariantCulture),
                    Rating = play.Rating,
                    Screenwriter = play.Screenwriter,
                    Title = play.Title
                });
            }
            context.Plays.AddRange(plays);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            var castsDto = XmlDeserializer<CastImport>("Casts", xmlString);
            StringBuilder sb = new StringBuilder();
            List<Cast> casts = new List<Cast>();
            foreach (var cast in castsDto)
            {
                if (!IsValid(cast))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var mainOrLesser = cast.IsMainCharacter ? "main" : "lesser";
                sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, mainOrLesser));
                casts.Add(new Cast()
                {
                    FullName = cast.FullName,
                    IsMainCharacter = cast.IsMainCharacter,
                    PhoneNumber = cast.PhoneNumber,
                    PlayId = cast.PlayId
                });
            }
            context.Casts.AddRange(casts);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            var theatreDto = JsonConvert.DeserializeObject<TheatreImport[]>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Theatre> theatres = new List<Theatre>();
            foreach (var theatre in theatreDto)
            {
                if (!IsValid(theatre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                List<Ticket> tickets = new List<Ticket>();
                foreach (var ticket in theatre.Tickets)
                {
                    if (!IsValid(ticket))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    tickets.Add(new Ticket()
                    {
                        RowNumber = ticket.RowNumber,
                        PlayId = ticket.PlayId,
                        Price = ticket.Price,
                    });
                }
                Theatre theatreToAdd = new Theatre()
                {
                    Director = theatre.Director,
                    Name = theatre.Name,
                    NumberOfHalls = theatre.NumberOfHalls,
                    Tickets = tickets
                };
                sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, tickets.Count));
                theatres.Add(theatreToAdd);
            }
            context.Theatres.AddRange(theatres);
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
        private static T[] XmlDeserializer<T>(string rootAttribute, string stringXml)
        {
            XmlRootAttribute root = new XmlRootAttribute(rootAttribute);
            XmlSerializer serializer = new XmlSerializer(typeof(T[]), root);
            var reader = new StringReader(stringXml);

            var deserialized = serializer.Deserialize(reader) as T[];
            return deserialized;
        }
    }
}
