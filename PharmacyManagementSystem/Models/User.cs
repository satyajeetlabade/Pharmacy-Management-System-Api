using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PharmacyManagementSystem.Enums;

namespace PharmacyManagementSystem.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(150, ErrorMessage = "Email can't exceed 150 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Phone(ErrorMessage = "Invalid contact number format.")]
        [StringLength(20, ErrorMessage = "Contact number can't exceed 20 characters.")]
        public string? Contact { get; set; }

        [Required]
        [EnumDataType(typeof(UserRole))]
        public UserRole Role { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
