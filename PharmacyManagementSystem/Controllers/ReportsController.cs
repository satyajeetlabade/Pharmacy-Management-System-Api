using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.Interfaces;

namespace PharmacyManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        
        public ReportsController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet("sales-summary")]
        public async Task<IActionResult> GetSalesSummary([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            if (fromDate == default || toDate == default)
                return BadRequest(new { error = "Both FromDate and ToDate must be provided." });

            if (fromDate > toDate)
                return BadRequest(new { error = "FromDate must be earlier than ToDate." });

            var result = await _reportRepository.GetSalesSummaryAsync(fromDate, toDate);
            return Ok(result);
        }




    }


}
