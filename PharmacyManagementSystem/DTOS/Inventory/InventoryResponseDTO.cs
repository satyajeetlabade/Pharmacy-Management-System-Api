namespace PharmacyManagementSystem.DTOS.Inventory
{
    public class InventoryResponseDTO
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public string DrugName { get; set; } = null!;
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = null!;
        public string BatchNumber { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
