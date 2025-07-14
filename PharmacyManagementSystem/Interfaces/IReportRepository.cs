
namespace PharmacyManagementSystem.Interfaces
{
    public interface IReportRepository
    {
        Task<SalesSummaryDto> GetSalesSummaryAsync(DateTime fromDate, DateTime toDate);

    }
}
