namespace Rentaly.DtoLayer.PageDtos;

public class ErrorPageDto
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
