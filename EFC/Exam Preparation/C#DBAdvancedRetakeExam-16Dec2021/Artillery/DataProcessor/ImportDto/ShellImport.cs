using Artillery.Data.Constants;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Shell")]
    public class ShellImport
    {
        [Required]
        [Range(2, 1680)]
        public double ShellWeight { get; set; }
        [Required]
        [MinLength(GlobalConstant.MANUFACTURER_CALIBER_MIN_LENGTH)]
        [MaxLength(GlobalConstant.MANUFACTURER_CALIBER_MAX_LENGTH)]
        public string Caliber { get; set; }
    }
}
