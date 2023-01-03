using Microsoft.AspNetCore.Mvc;
using SayingHelloLibrary.JsonRpc;
using SayingHelloWebApi.JsonRpc;

namespace SayingHelloWebApi.Controllers;

[ApiController]
[Route("/")]
public class SayingHelloController : ControllerBase
{
    private readonly IJsonRpcService _jsonRpcService;
    private readonly ILogger<SayingHelloController> _logger;

    public SayingHelloController(
        IJsonRpcService jsonRpcService,
        ILogger<SayingHelloController> logger
        )
    {
        _jsonRpcService = jsonRpcService;
        _logger = logger;
    }

    [HttpPost(Name = "PostSayingHello")]
    public async Task<JsonRpcResponse> Post()
    {
        Request.EnableBuffering();

        Request.Body.Position = 0;

        var json = await new StreamReader(Request.Body).ReadToEndAsync();

        return await _jsonRpcService.ProcessRequest(json, FunctionCalls.Dictionary);
    }
}
