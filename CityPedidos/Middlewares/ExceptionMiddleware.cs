using CityPedidos.Application.Exceptions;
using System.Text.Json;

namespace CityPedidos.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Antes de ejecutar la acción, validamos ModelState
                if (!context.Request.HasFormContentType && !context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                {
                    context.Request.EnableBuffering();
                }

                await _next(context);

                // Si ningún controller escribió respuesta y el status es 404
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await WriteErrorAsync(context, "Recurso no encontrado", 404);
                }
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            int statusCode = StatusCodes.Status500InternalServerError;

            if (ex is UnauthorizedAccessException)
                statusCode = StatusCodes.Status401Unauthorized;
            else if (ex is BadRequestException)
                statusCode = StatusCodes.Status400BadRequest;
            else if (ex is ArgumentException)
                statusCode = StatusCodes.Status400BadRequest;
            else if (ex is KeyNotFoundException)
                statusCode = StatusCodes.Status404NotFound;
            else if (ex is InvalidOperationException)
                statusCode = StatusCodes.Status403Forbidden;

            return WriteErrorAsync(context, ex.Message, statusCode);
        }


        private static Task WriteErrorAsync(HttpContext context, string errorMessage, int statusCode)
        {
            var response = new
            {
                ok = false,
                status = statusCode,
                error = errorMessage
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}
