using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ModelConfiguration.Models
{
    public class Color
    {
        public Color()
        {
            PrimaryKitTeams = new HashSet<Team>();
            SecondaryKitTeams = new HashSet<Team>();
        }
        [Key]
        public int ColorId { get; set; }
        public string Name { get; set; }
        public ICollection<Team> PrimaryKitTeams { get; set; }
        public ICollection<Team> SecondaryKitTeams { get; set; }
    }
}