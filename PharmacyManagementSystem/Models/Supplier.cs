using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PharmacyManagementSystem.Models
{
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Supplier name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(150, ErrorMessage = "Email address can't exceed 150 characters.")]
        public string? ContactEmail { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone number can't exceed 20 characters.")]
        public string? ContactNumber { get; set; }

        [StringLength(300, ErrorMessage = "Address can't exceed 300 characters.")]
        public string? Address { get; set; }

    }
}
