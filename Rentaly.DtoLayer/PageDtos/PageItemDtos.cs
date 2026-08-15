using Rentaly.EntityLayer.Enums;

namespace Rentaly.DtoLayer.PageDtos;

public class HomeContentItemDto
{
    public int HomeContentId { get; set; }
    public HomeSectionType Section { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

public class CarCardDto
{
    public int CarId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string BranchCity { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal DailyPrice { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int SeatCount { get; set; }
    public int LuggageCount { get; set; }
    public string FuelType { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsAvailable { get; set; }
}

public class BrandOptionDto
{
    public int BrandId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}

public class CarModelOptionDto
{
    public int CarModelId { get; set; }
    public int BrandId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
}

public class BranchOptionDto
{
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}
