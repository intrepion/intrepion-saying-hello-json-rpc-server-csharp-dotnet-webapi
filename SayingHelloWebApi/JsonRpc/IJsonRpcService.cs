using SayingHelloLibrary.JsonRpc;

namespace SayingHelloWebApi.JsonRpc
{
    public interface IJsonRpcService : IDisposable
    {
        Task<JsonRpcResponse> ProcessRequest(string json, Dictionary<string, FunctionCall> functionCalls);
    }
}
