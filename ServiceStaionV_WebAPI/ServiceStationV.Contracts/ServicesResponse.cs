namespace ServiceStationV.Contracts
{
    public record ServicesResponse(Guid Id, string Name, string Description, decimal Price, string ImagePath);
}
