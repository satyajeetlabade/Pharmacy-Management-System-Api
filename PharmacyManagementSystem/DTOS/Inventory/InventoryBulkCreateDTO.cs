namespace PharmacyManagementSystem.DTOS.Inventory
{
    public class InventoryBulkCreateDTO
    {
        public List<InventoryCreateDTO> Inventories { get; set; } = new();
    }
}
