namespace PharmacyManagementSystem.DTOS.Drugs
{
    public class DrugResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string DrugType { get; set; } = string.Empty;
    }
}
