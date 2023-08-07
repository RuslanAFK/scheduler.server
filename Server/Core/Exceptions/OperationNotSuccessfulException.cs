using System.Net;

namespace Domain.Exceptions;

public class OperationNotSuccessfulException : BaseException
{
    public override string Message { get; }
    public override HttpStatusCode  StatusCode { get; } = HttpStatusCode.BadRequest;
    public string Recommendation { get; } = "Please retry it later.";

    public OperationNotSuccessfulException(string message = "Operation is not successful.")
    {
        Message = $"{message} {Recommendation}";
    }
}