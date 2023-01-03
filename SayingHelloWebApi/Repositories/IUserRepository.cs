using SayingHelloLibrary.JsonRpc;

namespace SayingHelloWebApi.Repositories
{
    public interface IUserRepository : IDisposable
    {
        Task<JsonRpcResponse> LoginAsync(JsonRpcRequest request);
        Task<JsonRpcResponse> LogoutAsync(JsonRpcRequest request);
        Task<JsonRpcResponse> RegisterAsync(JsonRpcRequest request);
    }
}
