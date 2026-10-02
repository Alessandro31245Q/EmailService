using EmailService.Models;
using EmailService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Enviar(EmailRequest request)
    {
        if (request.Destinatarios.Count == 0) return BadRequest("Debe existir al menos un destinatario.");
        
        if (string.IsNullOrWhiteSpace(request.Asunto)) return BadRequest("El asunto es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Html)) return BadRequest("El contenido HTML es obligatorio.");
        
        await _emailService.EnviarAsync(request);

        return Ok(new{ mensaje = "Correo enviado correctamente."});
    }
}