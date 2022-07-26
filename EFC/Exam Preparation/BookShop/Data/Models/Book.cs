using BookShop.Data.Models.Constants;
using BookShop.Data.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Data.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(GlobalConstant.BOOK_NAME_MIN_LENGTH)]
        [MaxLength(GlobalConstant.BOOK_NAME_MAX_LENGTH)]
        public string Name { get; set; }
        [Required]
        public Genre Genre { get; set; }
        [Column(TypeName = "decimal(MAX,2)")]
        public decimal Price { get; set; }
        [Range(50,5000)]
        public int Pages { get; set; }
        [Required]
        public DateTime PublishedOn { get; set; }
        public AuthorBook AuthorsBooks { get; set; }
    }
}
