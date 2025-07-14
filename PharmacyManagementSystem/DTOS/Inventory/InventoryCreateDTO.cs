namespace PharmacyManagementSystem.DTOS.Inventory
{
    public class InventoryCreateDTO
    {
        public int DrugId { get; set; }
        public int SupplierId { get; set; }
        public string BatchNumber { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
