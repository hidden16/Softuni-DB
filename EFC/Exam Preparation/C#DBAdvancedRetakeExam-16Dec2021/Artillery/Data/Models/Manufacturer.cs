using Artillery.Data.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artillery.Data.Models
{
    public class Manufacturer
    {
        public Manufacturer()
        {
            Guns = new List<Gun>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(GlobalConstant.MANUFACTURER_MANUFACTURERNAME_MIN_LENGTH)]
        [MaxLength(GlobalConstant.MANUFACTURER_MANUFACTURERNAME_MAX_LENGTH)]
        public string ManufacturerName { get; set; }
        [Required]
        [MinLength(GlobalConstant.MANUFACTURER_FOUNDED_MIN_LENGTH)]
        [MaxLength(GlobalConstant.MANUFACTURER_FOUNDED_MAX_LENGTH)]
        public string Founded { get; set; }
        public ICollection<Gun> Guns { get; set; }
    }
}
