using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace VaporStore.DataProcessor.Dto.Import
{
    [XmlType("Purchase")]
    public class PurchaseExport
    {
        [Required]
        [XmlAttribute("title")]
        public string Title { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        [RegularExpression(@"[A-Z\d]*-[A-Z\d]*-[A-Z\d]*")]
        public string Key { get; set; }
        [Required]
        [RegularExpression(@"\d{4} \d{4} \d{4} \d{4}")]
        public string Card { get; set; }
        [Required]
        public string Date { get; set; }
    }
}
