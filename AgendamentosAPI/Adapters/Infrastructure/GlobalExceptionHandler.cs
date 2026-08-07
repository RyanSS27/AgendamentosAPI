using AgendamentosAPI.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendamentosAPI.Adapters.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Mapeamento da Exceção -> Status Code HTTP e Mensagem
        var (statusCode, title, detail) = exception switch
        {
            NotFoundException ex => (
                StatusCodes.Status404NotFound, 
                "Recurso não encontrado", 
                ex.Message
            ),
            DomainException ex => (
                StatusCodes.Status400BadRequest, 
                "Regra de negócio violada", 
                ex.Message
            ),
            DbUpdateConcurrencyException => (
                StatusCodes.Status409Conflict, 
                "Registro não existente", 
                "O registro foi removido por outro processo ou não existia previamente no banco de dados."
            ),
            _ => (
                StatusCodes.Status500InternalServerError, 
                "Erro interno do servidor", 
                "Ocorreu um erro inesperado. Entre em contato com o suporte."
            )
        };

        httpContext.Response.StatusCode = statusCode;

        // Padronização de resposta HTTP RFC 7807 (Problem Details)
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // Retorna true para sinalizar ao .NET que a exceção foi capturada e tratada
        return true; 
    }
}