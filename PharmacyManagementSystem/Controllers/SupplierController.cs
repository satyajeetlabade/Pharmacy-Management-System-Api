using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.DTOS.Suppliers;
using PharmacyManagementSystem.Interfaces;
using PharmacyManagementSystem.Models;

namespace PharmacyManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var suppliers = await _supplierRepository.GetAllAsync();

            var result = suppliers.Select(s => new SupplierResponseDto
            {
                Id = s.Id,
                Name = s.Name,
                ContactNumber = s.ContactNumber,
                ContactEmail = s.ContactEmail,
                Address = s.Address
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var supplier = await _supplierRepository.GetByIdAsync(id);
            if (supplier == null)
                return NotFound();

            var dto = new SupplierResponseDto
            {
                Id = supplier.Id,
                Name = supplier.Name,
                ContactEmail = supplier.ContactEmail,
                ContactNumber = supplier.ContactNumber,
                Address = supplier.Address
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SupplierCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var supplier = new Supplier
            {
                Name = dto.Name,
                ContactNumber = dto.ContactNumber,
                ContactEmail = dto.ContactEmail,
                Address = dto.Address
            };

            var created = await _supplierRepository.AddAsync(supplier);

            var response = new SupplierResponseDto
            {
                Id = created.Id,
                Name = created.Name,
                ContactNumber = created.ContactNumber,
                ContactEmail = created.ContactEmail,
                Address = created.Address
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        [HttpPut("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] SupplierUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var existing = await _supplierRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = dto.Name;
            existing.ContactEmail = dto.ContactEmail;
            existing.ContactNumber = dto.ContactNumber;
            existing.Address = dto.Address;

            await _supplierRepository.UpdateAsync(existing);

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _supplierRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _supplierRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
