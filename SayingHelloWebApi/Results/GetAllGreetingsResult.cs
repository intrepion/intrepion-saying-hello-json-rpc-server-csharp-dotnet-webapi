using SayingHelloWebApi.Entities;
using System.Text.Json.Serialization;

namespace SayingHelloWebApi.Results;

public class GetAllGreetingsResult
{
    [JsonPropertyName("greetings")]
    public List<Greeting> Greetings { get; set; }
}
