using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class OfficerPrisoner
    {
        [Required]
        public int PrisonerId { get; set; }
        [Required]
        public Prisoner Prisoner { get; set; }
        [Required]
        public int OfficerId { get; set; }
        [Required]
        public Officer Officer { get; set; }
    }
}
