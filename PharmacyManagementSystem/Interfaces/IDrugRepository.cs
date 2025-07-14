using PharmacyManagementSystem.Models;

namespace PharmacyManagementSystem.Interfaces
{
    public interface IDrugRepository
    {
        Task<IEnumerable<Drug>> GetAllAsync();
        Task<Drug?> GetByIdAsync(int id);
        Task<List<Drug>> SearchByNameAsync(string name);
        Task<Drug> AddAsync(Drug drug);
        Task UpdateAsync(Drug drug);
        Task DeleteAsync(int id);
    }
}
