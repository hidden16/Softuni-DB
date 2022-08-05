using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Constants;

namespace VaporStore.DataProcessor.Dto.Import
{
    public class UserImport
    {
        [Required]
        [RegularExpression(@"[A-Z][a-z]* [A-Z][a-z]*")]
        public string FullName { get; set; }
        [Required]
        [MinLength(GlobalConstant.USER_USERNAME_MIN_LENGTH)]
        [MaxLength(GlobalConstant.USER_USERNAME_MAX_LENGTH)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Range(GlobalConstant.USER_AGE_MIN_LENGTH, GlobalConstant.USER_AGE_MAX_LENGTH)]
        public int Age { get; set; }
        public UserCardImport[] Cards { get; set; }
    }
    public class UserCardImport
    {
        [Required]
        [RegularExpression(@"\d{4} \d{4} \d{4} \d{4}")]
        public string Number { get; set; }
        [Required]
        [RegularExpression(@"\d{3}")]
        [StringLength(3)]
        public string Cvc { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
