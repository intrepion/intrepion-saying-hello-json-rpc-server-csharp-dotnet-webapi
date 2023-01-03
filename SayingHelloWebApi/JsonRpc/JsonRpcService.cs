using Microsoft.AspNetCore.Identity;
using SayingHelloLibrary.JsonRpc;
using SayingHelloWebApi.Data;
using SayingHelloWebApi.Entities;
using SayingHelloWebApi.Repositories;
using System.Text.Json;

namespace SayingHelloWebApi.JsonRpc;

public class JsonRpcService : IJsonRpcService, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    private readonly IGreetingRepository _greetingRepository;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;

    public JsonRpcService(
        IConfiguration configuration,
        ApplicationDbContext context,
        IGreetingRepository greetingRepository,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IUserRepository userRepository
        )
    {
        _context = context;
        _configuration = configuration;
        _greetingRepository = greetingRepository;
        _signInManager = signInManager;
        _userManager = userManager;
        _userRepository = userRepository;
    }

    public async Task<JsonRpcResponse> ProcessRequest(string json, Dictionary<string, FunctionCall> functionCalls)
    {
        if (string.IsNullOrEmpty(json) || double.TryParse(json, out _))
        {
            return new JsonRpcResponse
            {
                JsonRpc = "2.0",
                Error = new JsonRpcError
                {
                    Code = -32600,
                    Message = "Invalid Request - json is not found",
                },
            };
        }

        try {
            var request = JsonSerializer.Deserialize<JsonRpcRequest>(json);

            if (request == null || string.IsNullOrEmpty(request.Method) || !functionCalls.ContainsKey(request.Method))
            {
                return new JsonRpcResponse
                {
                    JsonRpc = "2.0",
                    Error = new JsonRpcError
                    {
                        Code = -32601,
                        Message = "Method not found",
                    },
                };
            }

            FunctionCall functionCall = functionCalls[request.Method];

            if (functionCall.Parameters.Count > 0)
            {
                JsonElement paramsElement = request.Params;

                if (paramsElement.ValueKind == JsonValueKind.Object)
                {
                    foreach (var property in paramsElement.EnumerateObject())
                    {
                        if (property.Value.ValueKind == JsonValueKind.Null)
                        {
                            return new JsonRpcResponse
                            {
                                JsonRpc = "2.0",
                                Error = new JsonRpcError
                                {
                                    Code = -32602,
                                    Message = "Invalid params - value is null",
                                },
                            };
                        }
                        var parameter = functionCall.Parameters.First(p => p.Name == property.Name);
                        try {
                            switch (parameter.Kind)
                            {
                                case "int":
                                    parameter.Value = property.Value.GetInt32();
                                    break;
                                case "string":
                                    parameter.Value = property.Value.GetString();
                                    break;
                                default:
                                    break;
                            }
                        } catch (InvalidOperationException) {
                            return new JsonRpcResponse
                            {
                                JsonRpc = "2.0",
                                Error = new JsonRpcError
                                {
                                    Code = -32602,
                                    Message = "Invalid params - value is not of the correct type",
                                },
                            };
                        }
                    }
                }
            }

            if (request.Method == "get_all_greetings") {
                return await _greetingRepository.GetAllGreetingsAsync(request);
            } else if (request.Method == "login") {
                return await _userRepository.LoginAsync(request);
            } else if (request.Method == "logout") {
                return await _userRepository.LogoutAsync(request);
            } else if (request.Method == "new_greeting") {
                return await _greetingRepository.NewGreetingAsync(request);
            } else if (request.Method == "register") {
                return await _userRepository.RegisterAsync(request);
            }

        } catch (JsonException) {
            return new JsonRpcResponse
            {
                JsonRpc = "2.0",
                Error = new JsonRpcError
                {
                    Code = -32700,
                    Message = "Parse error",
                },
            };
        } catch (Exception exception) {
            return new JsonRpcResponse
            {
                JsonRpc = "2.0",
                Error = new JsonRpcError
                {
                    Code = -32602,
                    Message = "Invalid Request - internal error",
                    Data = exception,
                },
            };
        }

        return new JsonRpcResponse
        {
            JsonRpc = "2.0",
            Error = new JsonRpcError
            {
                Code = -32600,
                Message = "Invalid Request",
            }
        };
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
