using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ModelConfiguration.Models
{
    public class Town
    {
        public Town()
        {
            Teams = new HashSet<Team>();
        }
        [Key]
        public int TownId { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}