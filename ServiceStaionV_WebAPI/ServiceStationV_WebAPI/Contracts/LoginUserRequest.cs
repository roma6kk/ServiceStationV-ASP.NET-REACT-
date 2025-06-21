using System.ComponentModel.DataAnnotations;

namespace ServiceStationV_WebAPI.Contracts
{
    public record LoginUserRequest([Required] string PhoneNumber, [Required] string Password);
}
