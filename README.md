# EmailService

Microservicio REST desarrollado con **ASP.NET Core .NET 10** para el envío de correos electrónicos mediante la API de **Brevo**.

El servicio está diseñado para ser consumido por diferentes aplicaciones, permitiendo centralizar el envío de correos sin exponer las credenciales de Brevo en aplicaciones frontend como Vue, React o Flutter.

## 🚀 Tecnologías

* **.NET 10**
* **ASP.NET Core Web API**
* **C#**
* **Brevo API**
* **HttpClient**
* **Dependency Injection**
* **OpenAPI**
* **Docker**

## 📌 Características

* Envío de correos electrónicos mediante Brevo.
* Soporte para múltiples destinatarios.
* Contenido HTML.
* Asunto configurable.
* Configuración mediante `appsettings` y User Secrets.
* Separación entre Controller, Service y configuración.
* API reutilizable desde diferentes aplicaciones.
* Preparado para ejecutarse mediante Docker.

## 🏗️ Arquitectura

El proyecto utiliza una estructura sencilla basada en servicios:

```text
EmailService
│
├── Configuration
│   └── BrevoOptions.cs
│
├── Controllers
│   └── EmailController.cs
│
├── Models
│   └── EmailRequest.cs
│
├── Services
│   ├── IEmailService.cs
│   └── BrevoEmailService.cs
│
├── Properties
│
├── Dockerfile
├── Program.cs
├── appsettings.json
└── EmailService.csproj
```

### Flujo

```text
Aplicación cliente
       │
       │ POST /api/Email/send
       ▼
EmailController
       │
       ▼
IEmailService
       │
       ▼
BrevoEmailService
       │
       │ Brevo API
       ▼
    Brevo
       │
       ▼
Correo electrónico
```

## ⚙️ Requisitos

Antes de ejecutar el proyecto necesitas:

* [.NET 10 SDK](https://dotnet.microsoft.com/)
* Una cuenta de [Brevo](https://www.brevo.com/)
* Una API Key de Brevo.
* Un correo remitente autorizado/verificado en Brevo.

## 🔐 Configuración

Las credenciales de Brevo **no deben almacenarse directamente en el repositorio**.

La aplicación utiliza la sección `Brevo` para obtener:

```json
{
  "Brevo": {
    "ApiKey": "TU_API_KEY",
    "SenderEmail": "correo@dominio.com",
    "SenderName": "Mi Aplicación"
  }
}
```

### Opción recomendada: User Secrets

Durante desarrollo local puedes utilizar User Secrets.

Ejecuta:

```bash
dotnet user-secrets set "Brevo:ApiKey" "TU_API_KEY"
dotnet user-secrets set "Brevo:SenderEmail" "correo@dominio.com"
dotnet user-secrets set "Brevo:SenderName" "Mi Aplicación"
```

De esta manera, las credenciales permanecen fuera del repositorio.

> **Importante:** nunca publiques una API Key de Brevo en GitHub.

## ▶️ Ejecutar el proyecto

Clona el repositorio:

```bash
git clone https://github.com/Alessandro31245Q/EmailService.git
```

Ingresa al proyecto:

```bash
cd EmailService
```

Restaura las dependencias:

```bash
dotnet restore
```

Ejecuta la aplicación:

```bash
dotnet run
```

La API estará disponible en la URL indicada por ASP.NET Core al iniciar la aplicación.

## 📡 Endpoint

### Enviar correo

```http
POST /api/Email/send
```

### Request

```json
{
  "destinatarios": [
    "usuario1@correo.com",
    "usuario2@correo.com"
  ],
  "asunto": "Prueba de correo",
  "html": "<h1>Hola</h1><p>Este correo fue enviado mediante EmailService.</p>"
}
```

### Respuesta exitosa

```json
{
  "mensaje": "Correo enviado correctamente."
}
```

HTTP Status:

```text
200 OK
```

## ❌ Validaciones

El endpoint valida que:

* Exista al menos un destinatario.
* El asunto no esté vacío.
* El contenido HTML no esté vacío.

Ejemplo de respuesta cuando no existen destinatarios:

```text
Debe existir al menos un destinatario.
```

## 🧪 Ejemplo con cURL

```bash
curl -X POST "http://localhost:5250/api/Email/send" \
  -H "Content-Type: application/json" \
  -d '{
    "destinatarios": [
      "usuario@correo.com"
    ],
    "asunto": "Correo de prueba",
    "html": "<h1>Hola</h1><p>Correo enviado desde EmailService.</p>"
  }'
```

## 💻 Ejemplo desde JavaScript / Vue

```javascript
const response = await fetch("http://localhost:5250/api/Email/send", {
  method: "POST",
  headers: {
    "Content-Type": "application/json"
  },
  body: JSON.stringify({
    destinatarios: [
      "usuario@correo.com"
    ],
    asunto: "Correo desde Vue",
    html: `
      <h1>Hola</h1>
      <p>Este correo fue enviado utilizando EmailService.</p>
    `
  })
});

const data = await response.json();

console.log(data);
```

## 🐳 Docker

El proyecto incluye un `Dockerfile`, por lo que puede ejecutarse dentro de un contenedor.

Construir la imagen:

```bash
docker build -t email-service .
```

Ejecutar el contenedor:

```bash
docker run -p 8080:8080 email-service
```

Para producción, las variables sensibles deben proporcionarse mediante variables de entorno o mecanismos seguros de configuración.

## 🔒 Seguridad

Este proyecto está pensado para funcionar como un servicio backend independiente.

La aplicación cliente **no necesita conocer la API Key de Brevo**.

```text
❌ Vue
   └── API Key de Brevo

✅ Vue
   │
   │ Request
   ▼
EmailService
   │
   │ API Key
   ▼
Brevo
```

Esto permite mantener las credenciales fuera del frontend.

### Recomendaciones

* No subir API Keys al repositorio.
* Utilizar User Secrets durante desarrollo.
* Utilizar variables de entorno o secretos del proveedor durante producción.
* No almacenar credenciales directamente en `appsettings.json`.
* Utilizar HTTPS en producción.
* Configurar autenticación/autorización si el microservicio será expuesto públicamente.

## 🔧 Configuración de Brevo

El servicio utiliza la API SMTP de Brevo:

```text
POST https://api.brevo.com/v3/smtp/email
```

La comunicación con Brevo se realiza mediante `HttpClient`.

El servicio construye una solicitud con:

```json
{
  "sender": {
    "email": "remitente@dominio.com",
    "name": "Nombre del remitente"
  },
  "to": [
    {
      "email": "destinatario@correo.com"
    }
  ],
  "subject": "Asunto",
  "htmlContent": "<h1>Contenido</h1>"
}
```

## 📦 Uso como microservicio

La principal finalidad de este proyecto es poder reutilizar el servicio desde diferentes aplicaciones.

Por ejemplo:

```text
                 ┌─────────────────┐
                 │   Vue App       │
                 └────────┬────────┘
                          │
                          │ HTTP
                          ▼
                 ┌─────────────────┐
                 │  EmailService   │
                 │   ASP.NET Core  │
                 └────────┬────────┘
                          │
                          │ API
                          ▼
                 ┌─────────────────┐
                 │     Brevo       │
                 └────────┬────────┘
                          │
                          ▼
                     📧 Email
```

Esto permite que una misma API pueda ser utilizada posteriormente por:

* Aplicaciones Vue.
* Aplicaciones React.
* Aplicaciones Flutter.
* Otros microservicios .NET.
* Aplicaciones móviles.
* Procesos automatizados.

## 🛠️ Próximas mejoras

Algunas funcionalidades que pueden incorporarse posteriormente:

* Autenticación del microservicio.
* Plantillas de correo.
* Archivos adjuntos.
* CC y BCC.
* Registro de correos enviados.
* Manejo centralizado de errores.
* Logs.
* Reintentos automáticos.
* Cola de mensajes.
* Rate limiting.
* Health checks.
* Integración con Docker Compose.
* Despliegue automatizado mediante CI/CD.

## 📄 Licencia

Este proyecto es de uso personal y educativo.

---

**EmailService**
Microservicio de envío de correos desarrollado con ASP.NET Core y Brevo.
