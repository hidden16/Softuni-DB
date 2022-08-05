using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models.Enums;

namespace VaporStore.Data.Models
{
    public class Card
    {
        [Key]
        public int Id { get; set; }
        [Required]
        // requires regex for validation -> 1234 5678 9012 3456
        public string Number { get; set; }
        [Required]
        [StringLength(3)]
        public string Cvc { get; set; }
        [Required]
        public CardType Type { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public User User { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
    }
}
