using System.Text.Json.Serialization;
using PharmacyManagementSystem.Enums;

namespace PharmacyManagementSystem.DTOS.Drugs
{
    public class DrugCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DrugType DrugType { get; set; }
    }
}
