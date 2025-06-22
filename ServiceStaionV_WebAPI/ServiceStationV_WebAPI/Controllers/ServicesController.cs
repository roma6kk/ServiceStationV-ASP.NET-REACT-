using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceStationV.Application.Services;
using ServiceStationV.Contracts;
using ServiceStationV.Core.Abstractions;
using ServiceStationV.Core.Models;

namespace ServiceStationV_WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServicesController : ControllerBase
    {
        private IServicesService _servicesService { get; set; }
        public ServicesController(IServicesService servicesService)
        {
            _servicesService = servicesService;
        }
        [HttpGet]
        public async Task<ActionResult<List<ServicesResponse>>> GetServices()
        {
            var services = await _servicesService.GetAllServices();

            var response = services.Select(s => new ServicesResponse(s.Id, s.Name, s.Description, s.Price, s.ImagePath));

            return Ok(response);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<ServicesResponse>> GetService(Guid id)
        {
            var service = await _servicesService.GetServiceById(id);

            return Ok(new ServicesResponse(service.Id, service.Name, service.Description, service.Price, service.ImagePath));
        }
        [Authorize(Roles = ServiceStationV.Core.Models.User.ADMIN_ROLE)]
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateService([FromBody] ServicesRequest request)
        {
            var (service, error) = Service.Create(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                request.Price,
                request.ImagePath);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            await _servicesService.CreateService(service);

            return Ok(service);
        }
        [Authorize(Roles = ServiceStationV.Core.Models.User.ADMIN_ROLE)]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateService(Guid id, [FromBody] ServicesRequest request)
        {
            var serviceId = await _servicesService.UpdateService(id, request.Name, request.Description, request.Price, request.ImagePath);
            return Ok(serviceId);
        }
        [Authorize(Roles = ServiceStationV.Core.Models.User.ADMIN_ROLE)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteService(Guid id)
        {
            return Ok(await _servicesService.DeleteService(id));
        }

    }
}
