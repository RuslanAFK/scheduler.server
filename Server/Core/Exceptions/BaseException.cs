using System.Net;

namespace Domain.Exceptions;

public abstract class BaseException : Exception
{
    public override string Message { get; }
    public abstract HttpStatusCode StatusCode { get; }
}