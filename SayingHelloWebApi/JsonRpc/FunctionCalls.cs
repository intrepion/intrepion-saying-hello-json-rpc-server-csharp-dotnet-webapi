namespace SayingHelloWebApi.JsonRpc;

public static class FunctionCalls
{
    public static Dictionary<string, FunctionCall> Dictionary = new Dictionary<string, FunctionCall>
    {
        {
            "get_all_greetings", new FunctionCall
            {
                Parameters = new List<Parameter> {}
            }
        },
        {
            "login", new FunctionCall
            {
                Parameters = new List<Parameter>
                {
                    new Parameter { Name = "username", Kind = "string" },
                    new Parameter { Name = "password", Kind = "string" },
                }
            }
        },
        {
            "logout", new FunctionCall
            {
                Parameters = new List<Parameter> {}
            }
        },
        {
            "new_greeting", new FunctionCall
            {
                Parameters = new List<Parameter>
                {
                    new Parameter { Name = "name", Kind = "string" },
                }
            }
        },
        {
            "register", new FunctionCall
            {
                Parameters = new List<Parameter>
                {
                    new Parameter { Name = "confirm", Kind = "string" },
                    new Parameter { Name = "email", Kind = "string" },
                    new Parameter { Name = "password", Kind = "string" },
                    new Parameter { Name = "username", Kind = "string" },
                }
            }
        }
    };
}
