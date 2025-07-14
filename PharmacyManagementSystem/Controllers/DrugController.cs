using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.DTOS.Drugs;
using PharmacyManagementSystem.Interfaces;
using PharmacyManagementSystem.Models;

namespace PharmacyManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrugController : ControllerBase
    {
        private readonly IDrugRepository _drugRepository;

        public DrugController(IDrugRepository drugRepository)
        {
            _drugRepository = drugRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Doctor")] 
        public async Task<IActionResult> GetAll()
        {
            var drugs = await _drugRepository.GetAllAsync();

            var result = drugs.Select(d => new DrugResponseDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                QuantityInStock = d.QuantityInStock,
                ExpiryDate = d.ExpiryDate,
                DrugType = d.DrugType.ToString()
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetById(int id)
        {
            var drug = await _drugRepository.GetByIdAsync(id);
            if (drug == null)
                return NotFound();

            var dto = new DrugResponseDto
            {
                Id = drug.Id,
                Name = drug.Name,
                Description = drug.Description,
                Price = drug.Price,
                QuantityInStock = drug.QuantityInStock,
                ExpiryDate = drug.ExpiryDate,
                DrugType = drug.DrugType.ToString()
            };

            return Ok(dto);
        }

        [HttpGet("search")]
       [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> SearchDrugs([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { error = "Search term cannot be empty." });

            var results = await _drugRepository.SearchByNameAsync(name);

            if (results.Count == 0)
                return NotFound(new { message = "No matching drugs found." });

            return Ok(results); 
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] DrugCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var drug = new Drug
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                QuantityInStock = dto.QuantityInStock,
                ExpiryDate = dto.ExpiryDate,
                DrugType = dto.DrugType
            };

            var created = await _drugRepository.AddAsync(drug);

            var response = new DrugResponseDto
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                Price = created.Price,
                QuantityInStock = created.QuantityInStock,
                ExpiryDate = created.ExpiryDate,
                DrugType = created.DrugType.ToString()
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] DrugUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var existing = await _drugRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.Price = dto.Price;
            existing.QuantityInStock = dto.QuantityInStock;
            existing.ExpiryDate = dto.ExpiryDate;
            existing.DrugType = dto.DrugType;

            await _drugRepository.UpdateAsync(existing);

            return NoContent();
        }


        [HttpDelete("{id}")]
       [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _drugRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _drugRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
