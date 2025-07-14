using PharmacyManagementSystem.DTOS.Inventory;
using PharmacyManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PharmacyManagementSystem.Repositories
{
    public interface IInventoryRepository
    {
        Task<Inventory> AddAsync(Inventory inventory);
        Task AddBulkAsync(List<Inventory> inventories);
        Task<List<InventoryResponseDTO>> GetAllAsync();
        Task<InventoryResponseDTO?> GetByIdAsync(int id);
        Task<Inventory> UpdateAsync(Inventory inventory);
        Task DeleteAsync(int id);
    }
}
