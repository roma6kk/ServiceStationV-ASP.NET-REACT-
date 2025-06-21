namespace ServiceStationV_WebAPI.Contracts
{
    public record ServicesRequest(string Name, string Description, decimal Price, string ImagePath);
}
