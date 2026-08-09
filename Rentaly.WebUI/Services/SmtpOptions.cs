namespace Rentaly.WebUI.Services;

public class SmtpOptions
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FromAddress { get; set; } = "rezervasyon@rentaly.com";
    public string FromName { get; set; } = "Rentaly Rezervasyon";
    public bool EnableSsl { get; set; } = true;
}
