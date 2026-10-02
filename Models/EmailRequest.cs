namespace EmailService.Models;

public class EmailRequest
{
    public List<string> Destinatarios { get; set; } = [];

    public string Asunto { get; set; } = string.Empty;

    public string Html { get; set; } = string.Empty;
}