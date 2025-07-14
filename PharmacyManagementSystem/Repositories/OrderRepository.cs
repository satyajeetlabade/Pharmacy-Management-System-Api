using Microsoft.EntityFrameworkCore;
using PharmacyManagementSystem.Data;
using PharmacyManagementSystem.Enums;
using PharmacyManagementSystem.Interfaces;

namespace PharmacyManagementSystem.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Doctor)
                .Include(o => o.VerifiedBy)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Drug)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Doctor)
                .Include(o => o.VerifiedBy)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Drug)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> CreateAsync(Order order)
        {
            decimal total = 0;

            foreach (var item in order.OrderItems)
            {
                var drug = await _context.Drugs.FindAsync(item.DrugId);
                if (drug == null)
                    throw new Exception($"Drug with ID {item.DrugId} not found.");

                item.UnitPrice = drug.Price;
                total += drug.Price * item.Quantity;
            }

            order.OrderDate = DateTime.Now;
            order.Status = OrderStatus.Created;
            order.TotalAmount = total;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }


        public async Task VerifyOrderAsync(int orderId, int verifiedByAdminId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) throw new Exception("Order not found.");

            if (order.Status != OrderStatus.Created)
                throw new Exception("Only 'Created' orders can be verified.");

            var adminUser = await _context.Users.FindAsync(verifiedByAdminId);
            if (adminUser == null)
                throw new Exception($"Admin user with ID {verifiedByAdminId} does not exist.");

           

            order.VerifiedById = verifiedByAdminId;
            order.VerifiedAt = DateTime.UtcNow;
            order.Status = OrderStatus.Verified;

            await _context.SaveChangesAsync();
        }

        public async Task MarkOrderAsPickedUpAsync(int orderId, DateTime pickedUpAt)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Drug)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                throw new ArgumentException("Order not found.");
            }

            if (order.PickedUpAt != null)
            {
                throw new InvalidOperationException("Order has already been picked up.");
            }

            foreach (var item in order.OrderItems)
            {
                if (item.Drug.QuantityInStock < item.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for drug '{item.Drug.Name}'. Required: {item.Quantity}, Available: {item.Drug.QuantityInStock}");
                }
            }

            foreach (var item in order.OrderItems)
            {
                item.Drug.QuantityInStock -= item.Quantity;
            }

            order.PickedUpAt = pickedUpAt;
            order.Status = OrderStatus.PickedUp; 

            await _context.SaveChangesAsync();
        }


    }
}
