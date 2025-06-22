using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceStationV.Core.Abstractions;
using ServiceStationV.Core.Models;
using ServiceStationV.Contracts;
namespace ServiceStationV_WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<OrderResponse>>> GetOrders()
        {
            var orders = await _ordersService.GetAllOrders();
            var response = new List<OrderResponse>();

            foreach (var order in orders)
            {
                response.Add(new OrderResponse
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    VehicleInfo = order.VehicleInfo,
                    ServiceIds = order.ServiceIds.ToList(),
                    TotalPrice = order.TotalPrice,
                    Status = order.Status,
                    CreatedAt = order.CreatedAt,
                    UpdatedAt = order.UpdatedAt,
                    PlannedDate = order.PlannedDate,
                    CompletedAt = order.CompletedAt,
                    Comment = order.Comment
                });
            }

            return Ok(response);
        }

        [Authorize]
        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<OrderResponse>> GetOrder(Guid id)
        {
            var order = await _ordersService.GetOrderById(id);
            if (order == null)
                return NotFound();

            var response = new OrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                VehicleInfo = order.VehicleInfo,
                ServiceIds = order.ServiceIds.ToList(),
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                PlannedDate = order.PlannedDate,
                CompletedAt = order.CompletedAt,
                Comment = order.Comment
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateOrder([FromBody] OrderRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }


            var order = await _ordersService.CreateOrder(userId, request);
            return Ok(order);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateOrder(Guid id, [FromBody] OrderRequest request)
        {
            var success = await _ordersService.UpdateOrder(
                           id,
                           request.VehicleInfo,
                           request.ServiceIds,
                           request.TotalPrice,
                           request.Status,
                           request.PlannedDate,
                           request.CompletedAt,
                           request.Comment);

            if (!success)
                return NotFound();

            return Ok(id);
        }

        [Authorize(Roles = ServiceStationV.Core.Models.User.ADMIN_ROLE)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteOrder(Guid id)
        {
            var success = await _ordersService.DeleteOrder(id);
            if (!success)
                return NotFound();

            return Ok(id);
        }
    }
}
