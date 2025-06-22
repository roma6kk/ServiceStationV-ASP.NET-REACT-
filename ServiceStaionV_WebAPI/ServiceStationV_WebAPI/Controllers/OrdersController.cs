using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceStationV.Core.Abstractions;
using ServiceStationV.Core.Models;
using ServiceStationV.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ServiceStationV_WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;
        private readonly IServicesService _servicesService;

        public OrdersController(IOrdersService ordersService, IServicesService servicesService)
        {
            _ordersService = ordersService;
            _servicesService = servicesService;
        }

        [Authorize(Roles = ServiceStationV.Core.Models.User.ADMIN_ROLE)]
        [HttpGet]
        public async Task<ActionResult<List<OrderResponse>>> GetOrders()
        {
            var orders = await _ordersService.GetAllOrders();
            var response = new List<OrderResponse>();

            foreach (var order in orders)
            {
                var services = await _servicesService.GetServicesByIds(order.ServiceIds);

                var serviceResponses = services
                    .Select(s => new ServicesResponse(
                        s.Id,
                        s.Name,
                        s.Description,
                        s.Price,
                        s.ImagePath
                    ))
                    .ToList();

                response.Add(new OrderResponse
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    VehicleInfo = order.VehicleInfo,
                    ServiceItems = serviceResponses,
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
        [HttpGet("/{id:guid}/orders")]
        public async Task<ActionResult<List<OrderResponse>>> GetOrdersByUser(Guid id)
        {
            var orders = await _ordersService.GetOrdersByUserId(id);
            var response = new List<OrderResponse>();

            foreach (var order in orders)
            {
                var services = await _servicesService.GetServicesByIds(order.ServiceIds);

                var serviceResponses = services
                    .Select(s => new ServicesResponse(
                        s.Id,
                        s.Name,
                        s.Description,
                        s.Price,
                        s.ImagePath
                    ))
                    .ToList();

                response.Add(new OrderResponse
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    VehicleInfo = order.VehicleInfo,
                    ServiceItems = serviceResponses,
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
                return NotFound(new { Message = $"Заказ с ID {id} не найден." });

            var services = await _servicesService.GetServicesByIds(order.ServiceIds);

            var serviceResponses = services
                .Select(s => new ServicesResponse(
                    s.Id,
                    s.Name,
                    s.Description,
                    s.Price,
                    s.ImagePath
                ))
                .ToList();

            var response = new OrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                VehicleInfo = order.VehicleInfo,
                ServiceItems = serviceResponses,
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
                return Unauthorized("User ID not found in token.");
            }

            var orderId = await _ordersService.CreateOrder(userId, request);
            return Ok(orderId);
        }

        [Authorize]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateOrder(Guid id, [FromBody] OrderRequest request)
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
                return NotFound(new { Message = $"Заказ с ID {id} не найден." });

            return NoContent();
        }

        // Удаление заказа (только админ)
        [Authorize(Roles = ServiceStationV.Core.Models.User.ADMIN_ROLE)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            var success = await _ordersService.DeleteOrder(id);
            if (!success)
                return NotFound(new { Message = $"Заказ с ID {id} не найден." });

            return NoContent();
        }
    }
}