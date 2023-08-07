using System.Net;

namespace Domain.Exceptions;

public class EntityAlreadyExistsException : BaseException
{
    public override HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;
    public override string Message { get; }
    public string Entity { get; }
    public string PropertyName { get; }
    public string? PropertyValue { get; }
    public string Recommendation { get; } = "Consider changing it and trying again.";
    public EntityAlreadyExistsException(Type entityType, string propertyName, string propertyValue = null)
    {
        Entity = entityType.Name;
        PropertyName = propertyName;
        PropertyValue = propertyValue;
        Message = GenerateMessage();
    }
    private string GenerateMessage()
    {
        if (PropertyValue is null)
            return $"{Entity} with current {PropertyName} already exists. {Recommendation}";
        return @$"{Entity} with {PropertyName} ""{PropertyValue}"" already exists. {Recommendation}";
    }
}