using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Artillery.DataProcessor.ImportDto
{
    public class GunImport
    {
        [Required]
        public int ManufacturerId { get; set; }
        [Required]
        [Range(100, 1_350_000)]
        public int GunWeight { get; set; }
        [Required]
        [Range(2.00, 35.00)]
        public double BarrelLength { get; set; }
        public int? NumberBuild { get; set; }
        [Required]
        [Range(1, 100_000)]
        public int Range { get; set; }
        [Required]
        public string GunType { get; set; }
        [Required]
        public int ShellId { get; set; }
        public GunCountryImport[] Countries { get; set; }
    }
    public class GunCountryImport
    {
        public int Id { get; set; }
    }
}
