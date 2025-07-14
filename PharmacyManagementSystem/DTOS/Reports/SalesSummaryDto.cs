public class SalesSummaryDto
{
    public int TotalOrders { get; set; }
    public decimal TotalRevenue { get; set; }

    public int TotalItemsSold { get; set; }
    public decimal AverageOrderValue { get; set; }
    public int DistinctDrugsSold { get; set; }

    public List<ProductSalesDto> TopSellingDrugs { get; set; } = new();
    public List<DailySalesDto> SalesByDay { get; set; } = new();
}

public class ProductSalesDto
{
    public int DrugId { get; set; }
    public string DrugName { get; set; } = string.Empty;
    public int UnitsSold { get; set; }
    public decimal Revenue { get; set; }
}

public class DailySalesDto
{
    public DateTime Day { get; set; }
    public decimal Revenue { get; set; }
}
