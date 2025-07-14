namespace PharmacyManagementSystem.DTOS.Orders
{
    public class OrderCreateDto
    {
        public int DoctorId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
