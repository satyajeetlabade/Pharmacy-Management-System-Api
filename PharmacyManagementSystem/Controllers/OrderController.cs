using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyManagementSystem.DTOS.Orders;
using PharmacyManagementSystem.Interfaces;
using PharmacyManagementSystem.Models;

namespace PharmacyManagementSystem.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Doctor")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderRepository.GetAllAsync();

            var result = orders.Select(order => new OrderResponseDto
            {
                Id = order.Id,
                Status = order.Status.ToString(),
                OrderDate = order.OrderDate,
                DoctorName = order.Doctor.Name,
                VerifiedBy = order.VerifiedBy?.Name,
                VerifiedAt = order.VerifiedAt,
                PickedUpAt = order.PickedUpAt,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(item => new OrderItemResponseDto
                {
                    DrugName = item.Drug.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
      [Authorize(Roles = "Admin, Doctor")]
        public async Task<ActionResult<OrderResponseDto>> GetById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return NotFound();

            var dto = new OrderResponseDto
            {
                Id = order.Id,
                Status = order.Status.ToString(),
                OrderDate = order.OrderDate,
                DoctorName = order.Doctor.Name,
                VerifiedBy = order.VerifiedBy?.Name,
                VerifiedAt = order.VerifiedAt,
                PickedUpAt = order.PickedUpAt,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(item => new OrderItemResponseDto
                {
                    DrugName = item.Drug.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<OrderResponseDto>> Create([FromBody] OrderCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = new Order
            {
                DoctorId = dto.DoctorId,
                OrderItems = dto.Items.Select(i => new OrderItem
                {
                    DrugId = i.DrugId,
                    Quantity = i.Quantity
                }).ToList()
            };

            var created = await _orderRepository.CreateAsync(order);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { message = "Order placed successfully", orderId = created.Id, totalAmount = created.TotalAmount });

        }


       [Authorize(Roles = "Admin")]
        [HttpPut("{id}/verify")]
        public async Task<IActionResult> Verify(int id, [FromBody] OrderVerifyDto dto)
        {

            try
            {
            await _orderRepository.VerifyOrderAsync(id, dto.VerifiedByAdminId);
            return Ok(new { message = "Order verified successfully" });

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/pickup")]
        public async Task<IActionResult> Pickup(int id, [FromBody] OrderPickupDto dto)
        {
            try
            {
                await _orderRepository.MarkOrderAsPickedUpAsync(id, dto.PickedUpAt);
                return Ok(new { message = "Order picked up successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
