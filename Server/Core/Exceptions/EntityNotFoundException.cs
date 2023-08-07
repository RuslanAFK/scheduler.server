using System.Net;

namespace Domain.Exceptions;

public class EntityNotFoundException : BaseException
{
    public string Entity { get; }
    public string PropertyName { get; }
    public string? PropertyValue { get; }
    public override string Message { get; }
    public override HttpStatusCode StatusCode { get; } = HttpStatusCode.NotFound;
    public string Recommendation { get; } = "Please check if you entered the correct data.";
    public EntityNotFoundException(Type entityType, string propertyName, string propertyValue=null)
    {
        Entity = entityType.Name;
        PropertyName = propertyName;
        PropertyValue = propertyValue;
        Message = GenerateMessage();
    }
    private string GenerateMessage()
    {
        if (PropertyValue is null)
            return $"{Entity} with current {PropertyName} is not found. {Recommendation}";
        return @$"{Entity} with {PropertyName} ""{PropertyValue}"" is not found. {Recommendation}";
    }
}