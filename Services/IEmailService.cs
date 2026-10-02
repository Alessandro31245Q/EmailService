using EmailService.Models;

namespace EmailService.Services;

public interface IEmailService
{
    Task EnviarAsync(EmailRequest request);
}