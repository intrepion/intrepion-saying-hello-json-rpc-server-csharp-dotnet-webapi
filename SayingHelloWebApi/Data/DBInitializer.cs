using SayingHelloWebApi.Entities;

namespace SayingHelloWebApi.Data;

public static class DBInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (context.Greetings.Any())
        {
            return;
        }

        var greetings = new Greeting[]
        {
            new Greeting
            {
                Message = "Hello, world!",
                Name = "",
            },
            new Greeting
            {
                Message = "Hello, Oliver!",
                Name = "Oliver",
            },
            new Greeting
            {
                Message = "Hello, James!",
                Name = "James",
            },
        };

        context.Greetings.AddRange(greetings);
        context.SaveChanges();
    }
}
