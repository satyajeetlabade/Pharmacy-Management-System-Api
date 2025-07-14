using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PharmacyManagementSystem.Enums;

namespace PharmacyManagementSystem.Models
{
    public class Drug
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Drug name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Drug name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description can be up to 500 characters long.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 100000, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity in stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int QuantityInStock { get; set; }

        [Required(ErrorMessage = "Expiry date is required.")]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        [Required(ErrorMessage = "Drug type is required.")]
        public DrugType DrugType { get; set; }

        public ICollection<OrderItem>? OrderItems { get; set; }

        public ICollection<Inventory>? Inventories { get; set; }

    }
}
