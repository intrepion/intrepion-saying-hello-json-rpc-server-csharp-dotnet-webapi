using System.Text.Json.Serialization;

namespace SayingHelloWebApi.Results;

public class NewGreetingResult
{
    [JsonPropertyName("message")]
    public string Message { get; set; }
}
