using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ModelConfiguration.Models
{
    public class User
    {
        public User()
        {
            Bets = new List<Bet>();
        }
        [Key]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        //[EmailAddress]
        public string Email { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public ICollection<Bet> Bets { get; set; }
    }
}