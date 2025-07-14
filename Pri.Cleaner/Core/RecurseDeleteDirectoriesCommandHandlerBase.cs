namespace Pri.Cleaner.Core;

/// <summary>
/// An abstract command RecurseDeleteDirectoriesCommand handler that recurses directories
/// </summary>
public abstract class RecurseDeleteDirectoriesCommandHandlerBase : IRecurseDeleteDirectoriesCommandHandler
{
	/// <summary>
	/// Execute
	/// </summary>
	/// <param name="command" cref="RecurseDeleteDirectoriesCommand"></param>
	public void Execute(RecurseDeleteDirectoriesCommand command)
	{
		OnStarted();
		foreach (var path in command.Paths)
		{
			var fs = Directory.EnumerateFiles(path, "*.csproj", SearchOption.AllDirectories);
			foreach (var f in fs)
			{
				var dir = Path.GetDirectoryName(f);
				if (string.IsNullOrWhiteSpace(dir))
				{
					continue;
				}

				OnDirectory(new DirectoryInfo(Path.Combine(dir, "obj")));
				OnDirectory(new DirectoryInfo(Path.Combine(dir, "bin")));
			}
		}
		OnCompleted();
	}

	protected abstract void OnDirectory(DirectoryInfo directory);
	protected virtual void OnStarted() { }
	protected virtual void OnCompleted() { }
}
