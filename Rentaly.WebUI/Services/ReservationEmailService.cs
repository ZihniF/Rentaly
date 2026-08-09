using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.Extensions.Options;
using Rentaly.DtoLayer.RentalDtos;

namespace Rentaly.WebUI.Services;

public class ReservationEmailService : IReservationEmailService
{
    private readonly SmtpOptions _options;
    private readonly ILogger<ReservationEmailService> _logger;
    private readonly IWebHostEnvironment _environment;
    public ReservationEmailService(IOptions<SmtpOptions> options, ILogger<ReservationEmailService> logger,
        IWebHostEnvironment environment)
        => (_options, _logger, _environment) = (options.Value, logger, environment);

    public async Task<bool> SendApprovalAsync(ResultRentalDto rental, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.Host))
        {
            _logger.LogWarning("SMTP yapılandırılmadığı için rezervasyon #{RentalId} onay e-postası gönderilmedi.", rental.RentalId);
            return false;
        }

        using var message = new MailMessage
        {
            From = new MailAddress(_options.FromAddress, _options.FromName),
            Subject = $"Rezervasyonunuz onaylandı • #{rental.RentalId}",
            IsBodyHtml = true
        };
        message.To.Add(rental.CustomerEmail);
        var htmlView = AlternateView.CreateAlternateViewFromString(
            BuildBody(rental), null, MediaTypeNames.Text.Html);
        var couponPath = Path.Combine(_environment.WebRootPath, "rentaly", "images", "email",
            "next-booking-discount.png");
        if (File.Exists(couponPath))
        {
            var coupon = new LinkedResource(couponPath, MediaTypeNames.Image.Png)
            {
                ContentId = "rentaly-discount",
                TransferEncoding = TransferEncoding.Base64
            };
            htmlView.LinkedResources.Add(coupon);
        }
        message.AlternateViews.Add(htmlView);
        using var client = new SmtpClient(_options.Host, _options.Port)
        {
            EnableSsl = _options.EnableSsl,
            Credentials = new NetworkCredential(_options.UserName, _options.Password)
        };
        await client.SendMailAsync(message, cancellationToken);
        return true;
    }

    private static string BuildBody(ResultRentalDto r) => $"""
        <div style="font-family:Arial,sans-serif;background:#f4f6f8;padding:32px;color:#1f2937">
          <div style="max-width:640px;margin:auto;background:white;border-radius:18px;overflow:hidden">
            <div style="background:#0d1117;color:white;padding:28px"><h1 style="margin:0">Yola çıkmaya hazırsınız.</h1></div>
            <div style="padding:30px"><p>Merhaba {WebUtility.HtmlEncode(r.CustomerFullName)},</p>
              <p><strong>{WebUtility.HtmlEncode(r.BrandName)} {WebUtility.HtmlEncode(r.ModelName)}</strong> rezervasyonunuz onaylandı.</p>
              <table style="width:100%;background:#f8fafc;padding:18px;border-radius:12px"><tr><td>Alış</td><td><strong>{r.PickupDate:dd.MM.yyyy HH:mm}</strong></td></tr><tr><td>İade</td><td><strong>{r.ReturnDate:dd.MM.yyyy HH:mm}</strong></td></tr><tr><td>Toplam</td><td><strong>{r.TotalPrice:N2} ₺</strong></td></tr></table>
              <p style="margin-top:24px"><img src="cid:rentaly-discount" width="560" alt="Size özel indirim kuponu" style="display:block;max-width:100%;height:auto;border-radius:14px"></p>
              <p style="margin-top:28px">İyi yolculuklar,<br><strong>Rentaly Rezervasyon Ekibi</strong><br><span style="color:#6b7280">Güvenli yolculuğunuz için yanınızdayız.</span></p>
            </div>
          </div>
        </div>
        """;
}
