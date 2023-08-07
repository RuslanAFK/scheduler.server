using System.Net;

namespace Domain.Exceptions;

public class WrongPasswordException : BaseException
{
    public override string Message { get; } = "Provided incorrect password.";
    public override HttpStatusCode StatusCode { get; } = HttpStatusCode.Unauthorized;
}