using System.Net.Http.Json;
using EmailService.Configuration;
using EmailService.Models;
using Microsoft.Extensions.Options;
namespace EmailService.Services;

public class BrevoEmailService : IEmailService
{
    private readonly HttpClient _httpClient;
     private readonly BrevoOptions _options;

    public BrevoEmailService(HttpClient httpClient,IOptions<BrevoOptions> options)
    {
        _httpClient = httpClient;
         _options = options.Value;
    }

    public async Task EnviarAsync(EmailRequest request)
    {
        var destinatarios = request.Destinatarios.Select(email => new { email }).ToList();

        var body = new
        {
            sender = new{ email = _options.SenderEmail,name = _options.SenderName },
            to = destinatarios,
            subject = request.Asunto,
            htmlContent = request.Html
        };

        using var requestMessage = new HttpRequestMessage(HttpMethod.Post,"https://api.brevo.com/v3/smtp/email");

        requestMessage.Headers.Add("api-key", _options.ApiKey);
        requestMessage.Content = JsonContent.Create(body);

        using var response = await _httpClient.SendAsync(requestMessage);

        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode) throw new HttpRequestException($"Brevo respondió {(int)response.StatusCode}: {responseContent}");
    }
}