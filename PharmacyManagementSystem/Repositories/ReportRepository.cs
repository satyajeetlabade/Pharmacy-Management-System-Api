using Microsoft.EntityFrameworkCore;
using PharmacyManagementSystem.Data;
using PharmacyManagementSystem.Enums;
using PharmacyManagementSystem.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyManagementSystem.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SalesSummaryDto> GetSalesSummaryAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var pickedOrdersQuery = _context.Orders
                    .Where(o => o.Status == OrderStatus.PickedUp &&
                                o.PickedUpAt >= fromDate && o.PickedUpAt <= toDate)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.Drug);

                var pickedOrders = await pickedOrdersQuery.ToListAsync();

                var totalRevenue = pickedOrders.Sum(o => o.TotalAmount);
                var totalOrders = pickedOrders.Count;
                var allItems = pickedOrders.SelectMany(o => o.OrderItems).ToList();
                var totalItems = allItems.Sum(oi => oi.Quantity);

                var topDrugs = allItems
                    .GroupBy(oi => new { oi.DrugId, oi.Drug.Name })
                    .Select(g => new ProductSalesDto
                    {
                        DrugId = g.Key.DrugId,
                        DrugName = g.Key.Name,
                        UnitsSold = g.Sum(x => x.Quantity),
                        Revenue = g.Sum(x => x.Quantity * x.UnitPrice)
                    })
                    .OrderByDescending(x => x.UnitsSold)
                    .Take(5)
                    .ToList();

                var daily = pickedOrders
                    .GroupBy(o => o.PickedUpAt.Value.Date)
                    .Select(g => new DailySalesDto
                    {
                        Day = g.Key,
                        Revenue = g.Sum(o => o.TotalAmount)
                    })
                    .OrderBy(x => x.Day)
                    .ToList();

                return new SalesSummaryDto
                {
                    TotalOrders = totalOrders,
                    TotalRevenue = totalRevenue,
                    TotalItemsSold = totalItems,
                    AverageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0,
                    DistinctDrugsSold = allItems.Select(i => i.DrugId).Distinct().Count(),
                    TopSellingDrugs = topDrugs,
                    SalesByDay = daily
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while generating the sales summary.", ex);
            }
        }
    }
}
