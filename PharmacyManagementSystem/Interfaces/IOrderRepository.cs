using PharmacyManagementSystem.Models;

namespace PharmacyManagementSystem.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task<Order> CreateAsync(Order order);
        Task VerifyOrderAsync(int orderId, int verifiedByAdminId);
        Task MarkOrderAsPickedUpAsync(int orderId, DateTime pickedUpAt);
    }
}
