using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VaporStore.Data.Constants;

namespace VaporStore.Data.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }
        [Required]
        [Column(TypeName = GlobalConstant.GAME_RELEASDE_DATE_TYPE)]
        public DateTime ReleaseDate { get; set; }
        [Required]
        public int DeveloperId { get; set; }
        [Required]
        public Developer Developer { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
        public ICollection<GameTag> GameTags { get; set; }
    }
}
