using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class CoachImport
    {
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }
        [Required]
        public string Nationality { get; set; }
        [XmlArray]
        public CoachFootballerImport[] Footballers { get; set; }
    }
    [XmlType("Footballer")]
    public class CoachFootballerImport
    {
        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        public string Name { get; set; }
        [Required]
        public string ContractStartDate { get; set; }
        [Required]
        public string ContractEndDate { get; set; }
        [Required]
        [Range(0,3)]
        public int PositionType { get; set; }
        [Required]
        [Range(0,4)]
        public int BestSkillType { get; set; }
    }
}
