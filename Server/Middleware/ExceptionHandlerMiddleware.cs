using System.ComponentModel.DataAnnotations;
using System.Net;
using Domain.Exceptions;
using Newtonsoft.Json;

namespace Server.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BaseException e)
        {
            await HandleBaseExceptionAsync(context, e);
        }
        catch (ValidationException e)
        {
            await HandleValidationExceptionAsync(context, e);
        }
        catch
        {
            await HandleUnexpectedExceptionAsync(context);
        }
    }
    private static Task HandleBaseExceptionAsync(HttpContext context, BaseException exception)
    {
        var code = exception.StatusCode;
        var result = JsonConvert.SerializeObject(new
        {
            error = exception.Message
        });
        return WriteErrorToJson(context, result, code);
    }
    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        var code = HttpStatusCode.BadRequest;
        var result = JsonConvert.SerializeObject(new
        {
            error = exception.Message
        });
        return WriteErrorToJson(context, result, code);
    }

    private static Task HandleUnexpectedExceptionAsync(HttpContext context)
    {
        var code = HttpStatusCode.InternalServerError;
        var message = "Unexpected error occurred.";
        var result = JsonConvert.SerializeObject(new
        {
            error = message
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