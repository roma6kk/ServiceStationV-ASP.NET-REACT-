using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceStationV.Application.Services;
using ServiceStationV.Contracts;
using ServiceStationV.Core.Abstractions;

namespace ServiceStationV_WebAPI.Controllers
{
    [Authorize(Roles = ServiceStationV.Core.Models.User.USER_ROLE)]
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private ICartService _cartService {  get; set; }
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpGet]
        public async Task<ActionResult<List<ServicesResponse>>> GetCart()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }
            var services = await _cartService.Get(userId);
            var response = services.Select(s => new ServicesResponse(s.Id, s.Name, s.Description, s.Price, s.ImagePath));

            return Ok(response);
        }

        [HttpPost("{serviceId}")]
        public async Task<IActionResult> AddToCart(Guid serviceId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            try
            {
                await _cartService.Add(serviceId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка добавления в корзину: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{serviceId}")]
        public async Task<IActionResult> RemoveFromCart(Guid serviceId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            try
            {
                await _cartService.Remove(serviceId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
