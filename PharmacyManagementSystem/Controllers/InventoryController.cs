using PharmacyManagementSystem.DTOS.Inventory;
using PharmacyManagementSystem.Models;
using PharmacyManagementSystem.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace PharmacyManagementSystem.Controllers
{
    //[Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryRepository _repository;

        public InventoryController(IInventoryRepository repository)
        {
            _repository = repository;
        }

        [HttpPost("add")]
        public async Task<ActionResult<InventoryResponseDTO>> Add([FromBody] InventoryCreateDTO dto)
        {
            if (dto.DrugId <= 0 || dto.SupplierId <= 0)
                return BadRequest(new { message = "Invalid DrugId or SupplierId." });

            var entity = new Inventory
            {
                DrugId = dto.DrugId,
                SupplierId = dto.SupplierId,
                BatchNumber = dto.BatchNumber,
                QuantityReceived = dto.Quantity,
                SuppliedDate = dto.PurchaseDate
            };
            var created = await _repository.AddAsync(entity);
            var response = await _repository.GetByIdAsync(created.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        [HttpPost("bulk-add")]
        public async Task<IActionResult> BulkAdd([FromBody] InventoryBulkCreateDTO dto)
        {
            var entities = new List<Inventory>();
            var failedItems = new List<string>();

            foreach (var item in dto.Inventories)
            {
                if (item.DrugId <= 0 || item.SupplierId <= 0)
                {
                    failedItems.Add($"Invalid entry for DrugId: {item.DrugId}, SupplierId: {item.SupplierId}");
                    continue;
                }
                entities.Add(new Inventory
                {
                    DrugId = item.DrugId,
                    SupplierId = item.SupplierId,
                    BatchNumber = item.BatchNumber,
                    QuantityReceived = item.Quantity,
                    SuppliedDate = item.PurchaseDate
                });
            }

            if (failedItems.Any())
                return BadRequest(new { message = "Some entries failed validation", failedItems });

            await _repository.AddBulkAsync(entities);
            return Ok(new { message = "Bulk inventory added successfully." });
        }


        [HttpGet]
        public async Task<ActionResult<List<InventoryResponseDTO>>> GetAll()
        {
            var list = await _repository.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryResponseDTO>> GetById(int id)
        {
            var dto = await _repository.GetByIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<InventoryResponseDTO>> Update(int id, [FromBody] InventoryUpdateDTO dto)
        {
            if (dto == null) return BadRequest("Invalid data.");

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return NotFound($"Inventory {id} not found.");

            var entity = new Inventory
            {
                Id = id,
                DrugId = dto.DrugId,
                SupplierId = dto.SupplierId,
                BatchNumber = dto.BatchNumber,
                QuantityReceived = dto.Quantity,
                SuppliedDate = dto.PurchaseDate
            };
            var updated = await _repository.UpdateAsync(entity);
            var response = await _repository.GetByIdAsync(updated.Id);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
