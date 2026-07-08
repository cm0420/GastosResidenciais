using System.Net;
using System.Text.Json;
using GastosResidenciais.Api.Exceptions;

namespace GastosResidenciais.Api.Middlewares;

/// <summary>
/// Middleware responsável por capturar exceções lançadas pelas camadas internas (Services/Repositories)
/// e traduzi-las em respostas HTTP padronizadas, evitando try/catch repetido em cada Controller.
/// </summary>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            NotFoundException => (HttpStatusCode.NotFound, exception.Message),
            BusinessRuleException => (HttpStatusCode.UnprocessableEntity, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "Ocorreu um erro inesperado ao processar a requisição.")
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            logger.LogError(exception, "Erro não tratado durante o processamento da requisição {Path}", context.Request.Path);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var payload = JsonSerializer.Serialize(new
        {
            status = (int)statusCode,
            erro = message
        });

        await context.Response.WriteAsync(payload);
    }
}
