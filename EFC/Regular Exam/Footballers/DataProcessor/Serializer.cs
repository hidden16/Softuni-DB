namespace Footballers.DataProcessor
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Data;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ExportDto;
    using Footballers.XmlAssistance;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var coachesDto = context.Coaches
                .Where(x => x.Footballers.Count > 0)
                .ToArray()
                .Select(c=> new CoachExport()
                {
                    FootballersCount = c.Footballers.Count,
                    CoachName = c.Name,
                    Footballers = c.Footballers
                    .ToArray()
                    .Select(f=> new CoachFootballerExport()
                    {
                        Name = f.Name,
                        Position = f.PositionType.ToString()
                    })
                    .OrderBy(x=>x.Name)
                    .ToArray()
                })
                .OrderByDescending(x=>x.FootballersCount)
                .ThenBy(x=>x.CoachName)
                .ToArray();
            return XAssist.Serialize(coachesDto, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teamsDto = context.Teams
                .Where(x => x.TeamsFootballers.Any(x => x.Footballer.ContractStartDate >= date))
                .ToArray()
                .Select(t => new TeamExport()
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                    .Where(x => x.Footballer.ContractStartDate >= date)
                    .OrderByDescending(x => x.Footballer.ContractEndDate)
                    .ThenBy(x=>x.Footballer.Name)
                    .ToArray()
                    .Select(f => new TeamFootballerExport()
                    {
                        FootballerName = f.Footballer.Name,
                        BestSkillType = f.Footballer.BestSkillType.ToString(),
                        PositionType = f.Footballer.PositionType.ToString(),
                        ContractStartDate = f.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                        ContractEndDate = f.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture)
                    })
                    .ToArray()
                })
                .OrderByDescending(x=>x.Footballers.Count())
                .ThenBy(x=>x.Name)
                .Take(5)
                .ToArray();
            return JsonConvert.SerializeObject(teamsDto);
        }
    }
}
