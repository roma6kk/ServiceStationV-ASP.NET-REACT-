namespace ServiceStationV.Contracts
{
    public record UserInfoResponse(Guid UserId, string UserName, string Email, string PhoneNumber);
}
