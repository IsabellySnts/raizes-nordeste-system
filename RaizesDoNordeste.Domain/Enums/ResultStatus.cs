using System.Text.Json.Serialization;

namespace RaizesDoNordeste.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ResultStatus
{
    Success,
    ValidationError,
    NotFound,
    Forbidden,
    InternalServerError
}
