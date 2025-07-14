using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PharmacyManagementSystem.Models
{
    public class Inventory
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int DrugId { get; set; }

        [ForeignKey(nameof(DrugId))]
        public Drug Drug { get; set; } = null!;

        [Required]
        public int SupplierId { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; } = null!;

        [Required(ErrorMessage = "Quantity received is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity received must be at least 1.")]
        public int QuantityReceived { get; set; }

        [Required(ErrorMessage = "Unit cost is required.")]
        [Range(0.01, 1000000, ErrorMessage = "Unit cost must be greater than 0.")]
        public decimal UnitCost { get; set; }

        [Required(ErrorMessage = "Batch number is required.")]
        [StringLength(50, ErrorMessage = "Batch number can't exceed 50 characters.")]
        public string BatchNumber { get; set; }

        [Required(ErrorMessage = "Supplied date is required.")]
        [DataType(DataType.Date)]
        public DateTime SuppliedDate { get; set; }

        [Required(ErrorMessage = "Expiry date is required.")]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }
    }
}
