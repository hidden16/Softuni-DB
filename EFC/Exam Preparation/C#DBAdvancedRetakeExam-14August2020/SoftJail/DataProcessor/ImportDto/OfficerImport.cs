using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto
{
    [XmlType("Officer")]
    public class OfficerImport
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Money { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Weapon { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [XmlArray]
        public OfficerPrisonImport[] Prisoners { get; set; }
    }
    [XmlType("Prisoner")]
    public class OfficerPrisonImport
    {
        [XmlAttribute]
        public int Id { get; set; }
    }
}
