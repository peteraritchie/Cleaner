using System.CommandLine;

using Pri.Cleaner.Core;
using Pri.CommandLineExtensions;
using Pri.ConsoleApplicationBuilder;
using Microsoft.Extensions.DependencyInjection;

namespace Pri.Cleaner;

/// <summary>
/// Entry point
/// </summary>
internal static class Program
{
    static int Main(string[] args)
    {
        var builder = ConsoleApplication.CreateBuilder(args);
		builder.Services.AddSingleton<TextWriter>(Console.Out);
		builder.Services.AddSingleton<EnumerateRecurseDeleteDirectoriesCommandHandler>();
		builder.Services.AddSingleton<RecurseDeleteDirectoriesCommandHandler>();
		builder.Services.AddSingleton<IValueNormalizer<double, long>, StorageSizeValueNormalizer>();

		builder.Services.AddCommand()
			.WithDescription("clean")
			.WithOption<bool>("--dry-run", "Only display what will be done if true.")
			.WithArgument<DirectoryInfo?>("path", "The path to the directory to recurse and clean.")
				.WithDefault(new DirectoryInfo("."))
			.WithHandler<CleanCommandHandler>();
        return builder.Build<RootCommand>().Invoke(args);
    }
}
