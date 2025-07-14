using PharmacyManagementSystem.DTOS.Inventory;
using PharmacyManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using PharmacyManagementSystem.Data;

namespace PharmacyManagementSystem.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Inventory> AddAsync(Inventory inventory)
        {
            var drug = await _context.Drugs.FindAsync(inventory.DrugId);
            if (drug == null)
                throw new ArgumentException($"Drug with ID {inventory.DrugId} not found.");

            drug.QuantityInStock += inventory.QuantityReceived;
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        public async Task AddBulkAsync(List<Inventory> inventories)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var inventory in inventories)
                {
                    var drug = await _context.Drugs.FindAsync(inventory.DrugId);
                    if (drug == null)
                        throw new ArgumentException($"Drug with ID {inventory.DrugId} not found.");

                    drug.QuantityInStock += inventory.QuantityReceived;
                    _context.Inventories.Add(inventory);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw new ApplicationException("An error occurred while adding bulk inventory.");
            }
        }

        public async Task<List<InventoryResponseDTO>> GetAllAsync()
        {
            return await _context.Inventories
                .Include(i => i.Drug)
                .Include(i => i.Supplier)
                .Select(i => new InventoryResponseDTO
                {
                    Id = i.Id,
                    DrugId = i.DrugId,
                    DrugName = i.Drug.Name,
                    SupplierId = i.SupplierId,
                    SupplierName = i.Supplier.Name,
                    BatchNumber = i.BatchNumber,
                    Quantity = i.QuantityReceived,
                    PurchaseDate = i.SuppliedDate
                })
                .ToListAsync();
        }

        public async Task<InventoryResponseDTO?> GetByIdAsync(int id)
        {
            return await _context.Inventories
                .Include(i => i.Drug)
                .Include(i => i.Supplier)
                .Where(i => i.Id == id)
                .Select(i => new InventoryResponseDTO
                {
                    Id = i.Id,
                    DrugId = i.DrugId,
                    DrugName = i.Drug.Name,
                    SupplierId = i.SupplierId,
                    SupplierName = i.Supplier.Name,
                    BatchNumber = i.BatchNumber,
                    Quantity = i.QuantityReceived,
                    PurchaseDate = i.SuppliedDate
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Inventory> UpdateAsync(Inventory inventory)
        {
            var existingInventory = await _context.Inventories.FindAsync(inventory.Id);
            if (existingInventory == null)
                throw new ArgumentException($"Inventory with ID {inventory.Id} not found.");

            _context.Inventories.Update(inventory);
            await _context.SaveChangesAsync();
            return inventory;
        }

        public async Task DeleteAsync(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory != null)
            {
                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Inventory with ID {id} not found.");
            }
        }
    }
}
