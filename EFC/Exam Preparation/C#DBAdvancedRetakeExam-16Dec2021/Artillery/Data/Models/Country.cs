using Artillery.Data.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models
{
    public class Country
    {
        public Country()
        {
            CountriesGuns = new List<CountryGun>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(GlobalConstant.COUNTRY_COUNTRYNAME_MIN_LENGTH)]
        [MaxLength(GlobalConstant.COUNTRY_COUNTRYNAME_MAX_LENGTH)]
        public string CountryName { get; set; }
        [Required]
        [Range(50_000, 10_000_000)]
        public int ArmySize { get; set; }
        public ICollection<CountryGun> CountriesGuns { get; set; }
    }
}
