using System.Text.Json.Serialization;

namespace SayingHelloWebApi.Params;

public class NewGreetingParams
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
