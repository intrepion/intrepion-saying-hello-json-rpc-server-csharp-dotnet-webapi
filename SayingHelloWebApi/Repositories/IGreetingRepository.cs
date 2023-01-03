using SayingHelloLibrary.JsonRpc;

namespace SayingHelloWebApi.Repositories
{
    public interface IGreetingRepository : IDisposable
    {
        Task<JsonRpcResponse> GetAllGreetingsAsync(JsonRpcRequest request);
        Task<JsonRpcResponse> NewGreetingAsync(JsonRpcRequest request);
    }
}
