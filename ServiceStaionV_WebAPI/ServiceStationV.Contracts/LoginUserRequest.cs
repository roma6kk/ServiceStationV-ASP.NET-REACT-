using System.ComponentModel.DataAnnotations;

namespace ServiceStationV.Contracts
{
    public record LoginUserRequest([Required] string PhoneNumber, [Required] string Password);
}
