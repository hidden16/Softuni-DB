using Artillery.Data.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Manufacturer")]
    public class ManufacturerImport
    {
        [Required]
        [Index(IsUnique = true)]
        [MinLength(GlobalConstant.MANUFACTURER_MANUFACTURERNAME_MIN_LENGTH)]
        [MaxLength(GlobalConstant.MANUFACTURER_MANUFACTURERNAME_MAX_LENGTH)]
        public string ManufacturerName { get; set; }
        [Required]
        [MinLength(GlobalConstant.MANUFACTURER_FOUNDED_MIN_LENGTH)]
        [MaxLength(GlobalConstant.MANUFACTURER_FOUNDED_MAX_LENGTH)]
        public string Founded { get; set; }
    }
}
