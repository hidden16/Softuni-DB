using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VaporStore.Data.Constants;
using VaporStore.Data.Models.Enums;

namespace VaporStore.Data.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public PurchaseType Type { get; set; }
        [Required]
        // requires regex for validation -> 3 pairs of 4 uppercase letters ex. "ABCD-EFGH-1J3K"
        public string ProductKey { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int CardId { get; set; }
        [Required]
        public Card Card { get; set; }
        [Required]
        public int GameId { get; set; }
        [Required]
        public Game Game { get; set; }
    }
}
