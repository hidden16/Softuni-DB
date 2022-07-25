using BookShop.Data.Models.Constants;
using System.ComponentModel.DataAnnotations;
namespace BookShop.Data.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(GlobalConstant.AUTHOR_FIRST_NAME_MIN_LENGTH)]
        [MaxLength(GlobalConstant.AUTHOR_FIRST_NAME_MAX_LENGTH)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(GlobalConstant.AUTHOR_LAST_NAME_MIN_LENGTH)]
        [MaxLength(GlobalConstant.AUTHOR_LAST_NAME_MAX_LENGTH)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(12)]
        public string Phone { get; set; }
        public AuthorBook AuthorsBooks { get; set; }
    }
}
