using Pri.Cleaner.Core;
using Pri.CommandLineExtensions;

namespace Pri.Cleaner;

/// <summary>
/// A clean command handler abstraction.
/// </summary>
public class CleanCommandHandler(
	RecurseDeleteDirectoriesCommandHandler handler,
	EnumerateRecurseDeleteDirectoriesCommandHandler dryRunHandler)
	: ICommandHandler<bool, DirectoryInfo?>
{
	public int Execute(bool isDryRun, DirectoryInfo? dir)
	{
		if (dir is null) return 1;
		var command = new RecurseDeleteDirectoriesCommand(dir.FullName);
		if (isDryRun)
		{
			dryRunHandler.Execute(command);
		}
		else
		{
			handler.Execute(command);
		}

		return 0;
	}
}
