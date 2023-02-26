using System.Net;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Server.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DbUpdateException e)
        {
            await HandleSbUpdateExceptionAsync(context, e);
        }
        catch (InvalidDataException ex)
        {
            await HandleInvalidDataExceptionAsync(context, ex);
        }
        catch (Exception e)
        {
            await HandleUnexpectedExceptionAsync(context, e);
        }
    }

    private static Task HandleUnexpectedExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = JsonConvert.SerializeObject(new
        {
            error = "Unexpected error occurred."
        });
        return WriteErrorToJson(context, result, code);
    }
    
    private static Task HandleInvalidDataExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.BadRequest;
        var result = JsonConvert.SerializeObject(new { error = exception.Message });
        return WriteErrorToJson(context, result, code);
    }
    
    private static Task HandleSbUpdateExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.BadRequest;
        var result = JsonConvert.SerializeObject(new
        {
            error = exception.InnerException?.Message ?? exception.Message
        });
        return WriteErrorToJson(context, result, code);
    }

    private static Task WriteErrorToJson(HttpContext context, string result, HttpStatusCode code)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}