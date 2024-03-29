﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ModelConfiguration.Models
{
    public class Position
    {
        public Position()
        {
            Players = new List<Player>();
        }
        [Key]
        public int PositionId { get; set; }
        public string Name { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}