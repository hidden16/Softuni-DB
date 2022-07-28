using Artillery.Data.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Artillery.Data.Models
{
    public class Shell
    {
        public Shell()
        {
            Guns = new LinkedList<Gun>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(2,1680)]
        public double ShellWeight { get; set; }
        [Required]
        [MinLength(GlobalConstant.MANUFACTURER_CALIBER_MIN_LENGTH)]
        [MaxLength(GlobalConstant.MANUFACTURER_CALIBER_MAX_LENGTH)]
        public string Caliber { get; set; }
        public ICollection<Gun> Guns { get; set; }
    }
}
