using System.Net;

namespace Domain.Exceptions;

public class PasswordNotParseableException : BaseException
{
    public override HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;
    public override string Message { get; } = 
        "Given password cannot be parsed. Please change its format.";

}