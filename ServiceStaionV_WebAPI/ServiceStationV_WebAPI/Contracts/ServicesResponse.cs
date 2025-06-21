namespace ServiceStationV_WebAPI.Contracts
{
    public record ServicesResponse(Guid Id, string Name, string Description, decimal Price, string ImagePath);
}
