using CustomerCrudApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace CustomerCrudApi.Dtos
{
    public class CustomerRequestDto
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        public string? Password { get; set; }

        [Required]
        [EnumDataType(typeof(CustomerRole))]
        public CustomerRole Role { get; set; }
    }
}
