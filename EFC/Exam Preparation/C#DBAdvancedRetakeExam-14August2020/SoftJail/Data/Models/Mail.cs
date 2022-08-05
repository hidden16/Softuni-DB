using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.Data.Models
{
    public class Mail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Sender { get; set; }
        [Required]
        // requires regex for validation -> only of letters, spaces and numbers, which ends with “ str.” 
        public string Address { get; set; }
        [Required]
        public int PrisonerId { get; set; }
        [Required]
        public Prisoner Prisoner { get; set; }
    }
}
