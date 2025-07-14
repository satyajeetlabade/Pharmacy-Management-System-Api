using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PharmacyManagementSystem.Enums;
using PharmacyManagementSystem.Models;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Doctor ID is required.")]
    public int DoctorId { get; set; }

    [ForeignKey(nameof(DoctorId))]
    public User Doctor { get; set; } = null!;

    [Required(ErrorMessage = "Order date is required.")]
    [DataType(DataType.DateTime)]
    public DateTime OrderDate { get; set; }

    [Required]
    [EnumDataType(typeof(OrderStatus))]
    public OrderStatus Status { get; set; } = OrderStatus.Created;

    public int? VerifiedById { get; set; }

    [ForeignKey(nameof(VerifiedById))]
    public User? VerifiedBy { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? VerifiedAt { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? PickedUpAt { get; set; }

    [Required(ErrorMessage = "Total amount is required.")]
    [Range(0.01, 1000000, ErrorMessage = "Total amount must be greater than zero.")]
    public decimal TotalAmount { get; set; }

    [Required]
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

}
