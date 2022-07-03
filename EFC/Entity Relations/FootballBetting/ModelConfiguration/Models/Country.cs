﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ModelConfiguration.Models
{
    public class Country
    {
        public Country()
        {
            Towns = new HashSet<Town>();
        }
        [Key]
        public int CountryId { get; set; }
        public string Name { get; set; }
        public ICollection<Town> Towns { get; set; }
    }
}