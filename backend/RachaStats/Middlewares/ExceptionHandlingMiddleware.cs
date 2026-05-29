using System.Net;
using System.Text.Json;
using RachaStats.Application.Common;
using RachaStats.Application.Common.Exceptions;

namespace RachaStats.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly  RequestDelegate _next;
    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException ae )
        {
            await ErrorResponser(context, StatusCodes.Status400BadRequest,"Erro de Validacao", ae.Message);
        }
        catch (NotFoundException ex)
        {
            await ErrorResponser(context, StatusCodes.Status404NotFound, "Recurso nao Encontrado", ex.Message);
        }
        catch (Exception ex)
        {
            await ErrorResponser(context, StatusCodes.Status500InternalServerError, "Erro iterno!", ex.Message);
        }

    }

    private static async Task ErrorResponser(HttpContext context , int httpStatusCode, string message, string erro)
    {
        context.Response.StatusCode = httpStatusCode;
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            Message = message,
            Errors = new List<string>() { erro }
        };
        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}