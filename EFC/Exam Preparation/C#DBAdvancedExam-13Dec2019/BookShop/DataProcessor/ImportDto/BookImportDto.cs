using BookShop.Data.Models.Constants;
using BookShop.Data.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ImportDto
{
    [XmlType("Book")]
    public class BookImportDto
    {
        [Required]
        [MinLength(GlobalConstant.BOOK_NAME_MIN_LENGTH)]
        [MaxLength(GlobalConstant.BOOK_NAME_MAX_LENGTH)]
        public string Name { get; set; }
        [Required]
        [Range(1,3)]
        public int Genre { get; set; }
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335")]
        public decimal Price { get; set; }
        [Range(50,5000)]
        public int Pages { get; set; }
        [Required]
        public string PublishedOn { get; set; }
    }
}
