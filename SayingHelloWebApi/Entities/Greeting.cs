using System.Text.Json.Serialization;

namespace SayingHelloWebApi.Entities;

public class Greeting
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}
