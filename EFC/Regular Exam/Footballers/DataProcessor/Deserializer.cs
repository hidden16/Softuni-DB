namespace Footballers.DataProcessor
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
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Footballers.XmlAssistance;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";


        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            var coachesDto = XAssist.Deserialize<CoachImport>("Coaches", xmlString);
            List<Coach> coaches = new List<Coach>();
            StringBuilder sb = new StringBuilder();
            foreach (var coach in coachesDto)
            {
                if (!IsValid(coach))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Coach coachToAdd = new Coach()
                {
                    Name = coach.Name,
                    Nationality = coach.Nationality
                };
                foreach (var footballer in coach.Footballers)
                {
                    if (!IsValid(footballer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var isStartDateValid = DateTime.TryParseExact(footballer.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate);
                    if (!isStartDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var isEndDateVlid = DateTime.TryParseExact(footballer.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate);
                    if (!isEndDateVlid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (startDate > endDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    coachToAdd.Footballers.Add(new Footballer()
                    {
                        ContractEndDate = endDate,
                        ContractStartDate = startDate,
                        BestSkillType = (BestSkillType)footballer.BestSkillType,
                        PositionType = (PositionType)footballer.PositionType,
                        Name = footballer.Name
                    });
                }
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coachToAdd.Footballers.Count));
                coaches.Add(coachToAdd);
            }
            context.Coaches.AddRange(coaches);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            var teamsDto = JsonConvert.DeserializeObject<TeamImport[]>(jsonString);
            List<Team> teams = new List<Team>();
            StringBuilder sb = new StringBuilder();
            foreach (var team in teamsDto)
            {
                if (!IsValid(team))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Team teamToAdd = new Team()
                {
                    Name = team.Name,
                    Nationality = team.Nationality,
                    Trophies = team.Trophies
                };
                foreach (var footballer in team.Footballers.Distinct())
                {
                    if (!context.Footballers.Any(x => x.Id == footballer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    teamToAdd.TeamsFootballers.Add(new TeamFootballer()
                    {
                        FootballerId = footballer,
                        Team = teamToAdd
                    });
                }
                sb.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, teamToAdd.TeamsFootballers.Count));
                teams.Add(teamToAdd);
            }
            context.Teams.AddRange(teams);
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
