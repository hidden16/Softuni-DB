using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Constants;

namespace VaporStore.Data.Models
{
    public class User
    {
        public User()
        {
            Cards = new List<Card>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(GlobalConstant.USER_USERNAME_MIN_LENGTH)]
        [MaxLength(GlobalConstant.USER_USERNAME_MAX_LENGTH)]
        public string Username { get; set; }
        [Required]
        // Requires regex for validation
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Range(GlobalConstant.USER_AGE_MIN_LENGTH, GlobalConstant.USER_AGE_MAX_LENGTH)]
        public int Age { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}
