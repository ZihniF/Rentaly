namespace Rentaly.DtoLayer.BrandDtos;

public class BrandManagementDto
{
    public int BrandId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<BrandModelItemDto> Models { get; set; } = [];
}

public class BrandModelItemDto
{
    public int CarModelId { get; set; }
    public string ModelName { get; set; } = string.Empty;
    public int VehicleCount { get; set; }
}
