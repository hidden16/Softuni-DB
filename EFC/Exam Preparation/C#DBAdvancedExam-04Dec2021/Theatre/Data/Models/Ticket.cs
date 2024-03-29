﻿using System.ComponentModel.DataAnnotations;

namespace Theatre.Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1.00,100.00)]
        public decimal Price { get; set; }
        [Required]
        [Range(1,10)]
        public sbyte RowNumber { get; set; }
        [Required]
        public int PlayId { get; set; }
        public Play Play { get; set; }
        [Required]
        public int TheatreId { get; set; }
        public Theatre Theatre { get; set; }
    }
}
