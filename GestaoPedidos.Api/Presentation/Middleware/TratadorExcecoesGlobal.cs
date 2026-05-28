using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GestaoPedidos.Api.Presentation.Middleware;

public class TratadorExcecoesGlobal : IExceptionHandler
{
    private readonly ILogger<TratadorExcecoesGlobal> _logger;

    public TratadorExcecoesGlobal(ILogger<TratadorExcecoesGlobal> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // FluentValidation
        if (exception is ValidationException falhaValidacao)
        {
            _logger.LogWarning("Falha de validação detectada: {Mensagem}", exception.Message);

            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            httpContext.Response.ContentType = "application/json";

            var erros = falhaValidacao.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => grupo.Select(e => e.ErrorMessage).ToArray()
                );

            var problema = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Um ou mais erros de validação ocorreram.",
                Detail = "Verifique a lista de erros para corrigir as informações enviadas.",
                Instance = httpContext.Request.Path
            };
            problema.Extensions.Add("erros", erros);

            await httpContext.Response.WriteAsJsonAsync(problema, cancellationToken);
            return true;
        }

        // Regras de Negócio
        if (exception is InvalidOperationException erroNegocio)
        {
            _logger.LogWarning("Violação de regra de negócio: {Mensagem}", exception.Message);

            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest; 
            httpContext.Response.ContentType = "application/json";

            var problema = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Operação Inválida.",
                Detail = erroNegocio.Message, 
                Instance = httpContext.Request.Path
            };

            await httpContext.Response.WriteAsJsonAsync(problema, cancellationToken);
            return true; 
        }

        // Qualquer outro erro
        _logger.LogError(exception, "Ocorreu um erro não tratado no sistema.");

        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        var erroInesperado = new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "Erro interno do servidor.",
            Detail = "Ocorreu um erro interno no nosso sistema. Tente novamente mais tarde.",
            Instance = httpContext.Request.Path
        };

        await httpContext.Response.WriteAsJsonAsync(erroInesperado, cancellationToken);
        return true; 
    }
}