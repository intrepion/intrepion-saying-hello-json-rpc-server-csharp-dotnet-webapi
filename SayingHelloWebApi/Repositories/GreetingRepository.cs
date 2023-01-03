using Microsoft.EntityFrameworkCore;
using SayingHelloLibrary.Domain;
using SayingHelloLibrary.JsonRpc;
using SayingHelloWebApi.Data;
using SayingHelloWebApi.Entities;
using SayingHelloWebApi.Params;
using SayingHelloWebApi.Results;
using System.Text.Json;

namespace SayingHelloWebApi.Repositories;

public class GreetingRepository : IGreetingRepository, IDisposable
{
    private readonly ApplicationDbContext _context;

    public GreetingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JsonRpcResponse> GetAllGreetingsAsync(JsonRpcRequest request)
    {
        var greetings = await _context.Greetings.ToListAsync();

        return new JsonRpcResponse
        {
            Id = request.Id,
            JsonRpc = request.JsonRpc,
            Result = new GetAllGreetingsResult
            {
                Greetings = greetings
            },
        };
    }

    public async Task<JsonRpcResponse> NewGreetingAsync(JsonRpcRequest request)
    {
        var newGreetingParams = JsonSerializer.Deserialize<NewGreetingParams>(request.Params.GetRawText());
        var name = newGreetingParams.Name.Trim();

        var greeting = await _context.Greetings.Where(greeting => greeting.Name == name).FirstOrDefaultAsync();
        if (greeting != null) {
            return new JsonRpcResponse
            {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Result = new NewGreetingResult
                {
                    Message = greeting.Message
                },
            };
        }

        var message = SayingHello.SayHello(name);

        greeting = new Greeting
        {
            Name = name,
            Message = message,
        };

        await _context.AddAsync(greeting);
        _context.SaveChanges();

        return new JsonRpcResponse
        {
            Id = request.Id,
            JsonRpc = request.JsonRpc,
            Result = new NewGreetingResult
            {
                Message = message
            },
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
