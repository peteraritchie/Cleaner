using Microsoft.Extensions.Logging;

using Pri.ConsoleApplicationBuilder;

namespace Pri.Cleaner;

class Program(ILogger<Program> logger)
{
    static void Main(string[] args)
    {
        var builder = ConsoleApplication.CreateBuilder(args);
        var program = builder.Build<Program>();
        program.Run();
    }

    private void Run()
    {
        logger.LogInformation("Hello, World!");
    }
}