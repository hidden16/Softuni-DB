using Artillery.Data.Constants;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Country")]
    public class CountryImport
    {
        [Required]
        [MinLength(GlobalConstant.COUNTRY_COUNTRYNAME_MIN_LENGTH)]
        [MaxLength(GlobalConstant.COUNTRY_COUNTRYNAME_MAX_LENGTH)]
        public string CountryName { get; set; }
        [Required]
        [Range(50_000, 10_000_000)]
        public int ArmySize { get; set; }
    }
}
