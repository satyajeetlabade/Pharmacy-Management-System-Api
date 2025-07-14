using Microsoft.EntityFrameworkCore;
using PharmacyManagementSystem.Data;
using PharmacyManagementSystem.Interfaces;
using PharmacyManagementSystem.Models;

namespace PharmacyManagementSystem.Repositories
{
    public class DrugRepository : IDrugRepository
    {
        private readonly ApplicationDbContext _context;

        public DrugRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Drug>> GetAllAsync()
        {
            try
            {
                return await _context.Drugs.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                throw new ApplicationException("An error occurred while retrieving drugs.");
            }
        }

        public async Task<Drug?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Drugs.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
            }
            catch (Exception)
            {
                throw new ApplicationException($"An error occurred while retrieving drug with ID {id}.");
            }
        }

        public async Task<List<Drug>> SearchByNameAsync(string name)
        {
            try
            {
                return await _context.Drugs
                    .Where(d => EF.Functions.Like(d.Name, $"%{name}%"))
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw new ApplicationException("An error occurred while searching drugs.");
            }
        }

        public async Task<Drug> AddAsync(Drug drug)
        {
            try
            {
                await _context.Drugs.AddAsync(drug);
                await _context.SaveChangesAsync();
                return drug;
            }
            catch (Exception)
            {
                throw new ApplicationException("An error occurred while adding the drug.");
            }
        }

        public async Task UpdateAsync(Drug drug)
        {
            try
            {
                var existingDrug = await _context.Drugs.FindAsync(drug.Id);
                if (existingDrug == null)
                {
                    throw new ArgumentException($"Drug with ID {drug.Id} not found.");
                }

                existingDrug.Name = drug.Name;
                existingDrug.Description = drug.Description;
                existingDrug.Price = drug.Price;
                existingDrug.QuantityInStock = drug.QuantityInStock;
                existingDrug.ExpiryDate = drug.ExpiryDate;
                existingDrug.DrugType = drug.DrugType;

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ApplicationException($"An error occurred while updating the drug with ID {drug.Id}");
            }
        }


        public async Task DeleteAsync(int id)
        {
            try
            {
                var drug = await _context.Drugs.FindAsync(id);
                if (drug == null)
                {
                    throw new ArgumentException($"Drug with ID {id} not found.");
                }
                _context.Drugs.Remove(drug);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ApplicationException($"An error occurred while deleting the drug with ID {id}.");
            }
        }
    }
}
