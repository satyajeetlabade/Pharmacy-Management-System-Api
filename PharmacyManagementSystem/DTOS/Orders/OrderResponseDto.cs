namespace PharmacyManagementSystem.DTOS.Orders
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string? VerifiedBy { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public DateTime? PickedUpAt { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemResponseDto> Items { get; set; } = new();
    }

    public class OrderItemResponseDto
    {
        public string DrugName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total => UnitPrice * Quantity;
    }

}
