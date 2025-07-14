using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PharmacyManagementSystem.Models
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;

        [Required]
        public int DrugId { get; set; }

        [ForeignKey(nameof(DrugId))]
        public Drug Drug { get; set; } = null!;

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit price is required.")]
        [Range(0.01, 1000000, ErrorMessage = "Unit price must be greater than zero.")]
        public decimal UnitPrice
        {
            get; set;
        }

        }
}
