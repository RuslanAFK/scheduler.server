using System.Net;

namespace Domain.Exceptions;

public class UserNotAuthorizedException : BaseException
{
    public override string Message { get; } = "User is not authorized.";
    public override HttpStatusCode StatusCode { get; } = HttpStatusCode.Unauthorized;
}