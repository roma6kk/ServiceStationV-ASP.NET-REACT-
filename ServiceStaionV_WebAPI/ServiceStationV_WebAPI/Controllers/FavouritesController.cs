using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceStationV.Core.Abstractions;
using ServiceStationV.Core.Models;
using ServiceStationV.DataAccess;
using ServiceStationV_WebAPI.Contracts;
using System;

namespace ServiceStationV_WebAPI.Controllers
{
    [Authorize(Roles = ServiceStationV.Core.Models.User.USER_ROLE)]
    [ApiController]
    [Route("[controller]")]
    public class FavouritesController : ControllerBase
    {
        private IFavouriteService _favouriteService { get; set; }
        public FavouritesController(IFavouriteService favouriteService)
        {
            _favouriteService = favouriteService;
        }
        [HttpGet]
        public async Task<ActionResult<List<ServicesResponse>>> GetFavourites()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }
            var services = await _favouriteService.Get(userId);
            var response = services.Select(s => new ServicesResponse(s.Id, s.Name, s.Description, s.Price, s.ImagePath));

            return Ok(services);
        }

        [HttpPost("{serviceId}")]
        public async Task<IActionResult> AddToFavourite(Guid serviceId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            try
            {
                await _favouriteService.Add(serviceId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка добавления в избранное: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{serviceId}")]
        public async Task<IActionResult> RemoveFromFavourites(Guid serviceId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("User ID not found in token");
            }

            try
            {
                await _favouriteService.Remove(serviceId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
