using Rentaly.DtoLayer.PageDtos;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Mappings;

internal static class PageDtoMappings
{
    public static HomeContentItemDto ToPageDto(this HomeContent value) => new()
    {
        HomeContentId = value.HomeContentId,
        Section = value.Section,
        Title = value.Title,
        Subtitle = value.Subtitle ?? string.Empty,
        Description = value.Description ?? string.Empty,
        ImageUrl = value.ImageUrl ?? string.Empty,
        Icon = value.Icon ?? string.Empty,
        DisplayOrder = value.DisplayOrder,
        IsActive = value.IsActive
    };

    public static CarCardDto ToCardDto(this Car value) => new()
    {
        CarId = value.CarId,
        BrandName = value.Brand?.BrandName ?? string.Empty,
        ModelName = value.Model?.ModelName ?? string.Empty,
        CategoryName = value.Category?.CategoryName ?? string.Empty,
        BranchId = value.BranchId,
        BranchName = value.Branch?.BranchName ?? string.Empty,
        BranchCity = value.Branch?.City ?? string.Empty,
        Year = value.Year,
        DailyPrice = value.DailyPrice,
        ImageUrl = value.ImageUrl,
        SeatCount = value.SeatCount,
        LuggageCount = value.LuggageCount,
        FuelType = value.FuelType,
        IsActive = value.IsActive,
        IsAvailable = value.IsAvailable
    };

    public static BrandOptionDto ToOptionDto(this Brand value) => new()
    {
        BrandId = value.BrandId,
        BrandName = value.BrandName,
        ImageUrl = value.ImageUrl
    };

    public static CarModelOptionDto ToOptionDto(this CarModel value) => new()
    {
        CarModelId = value.CarModelId,
        BrandId = value.BrandId,
        BrandName = value.Brand?.BrandName ?? string.Empty,
        ModelName = value.ModelName
    };

    public static BranchOptionDto ToOptionDto(this Branch value) => new()
    {
        BranchId = value.BranchId,
        BranchName = value.BranchName,
        City = value.City
    };
}
