namespace ServiceStationV_WebAPI.Contracts
{
    public record UsersRequest(string UserName, string Email, string PhoneNumber, string Password);
}
