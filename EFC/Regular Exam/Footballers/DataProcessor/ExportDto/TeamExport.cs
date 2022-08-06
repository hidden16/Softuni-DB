using System;
using System.Collections.Generic;
using System.Text;

namespace Footballers.DataProcessor.ExportDto
{
    public class TeamExport
    {
        public string Name { get; set; }
        public TeamFootballerExport[] Footballers { get; set; }
    }
    public class TeamFootballerExport
    {
        public string FootballerName { get; set; }
        public string ContractStartDate { get; set; }
        public string ContractEndDate { get; set; }
        public string BestSkillType { get; set; }
        public string PositionType { get; set; }
    }
}
