using System.IO;

namespace Pri.Cleaner.Core;

/// <summary>
/// A dry-run RecurseDeleteDirectoriesCommand handler
/// </summary>
public class EnumerateRecurseDeleteDirectoriesCommandHandler(TextWriter textWriter, IValueNormalizer<double, long> storageSizeNormalizer)
	: RecurseDeleteDirectoriesCommandHandlerBase
{
	private readonly List<DirectoryInfo> processedDirectories = [];

	/// <summary>
	/// Execute
	/// </summary>
	/// <param name="command" cref="RecurseDeleteDirectoriesCommand"></param>
	public void ExecuteO(RecurseDeleteDirectoriesCommand command)
	{
		var processedDirectories = new List<DirectoryInfo>();
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

				var targetDir = new DirectoryInfo(Path.Combine(dir, "obj"));
				if (targetDir.Exists)
				{
					processedDirectories.Add(targetDir);
					textWriter.WriteLine($"Would have removed directory {targetDir}");
				}
				targetDir = new DirectoryInfo(Path.Combine(dir, "bin"));
				if (!targetDir.Exists)
				{
					continue;
				}

				processedDirectories.Add(targetDir);
				textWriter.WriteLine($"Would have removed directory {targetDir}");
			}
		}

		long sum = processedDirectories.Sum(static e
			=>
		{
			return e.EnumerateFiles("*.*", SearchOption.AllDirectories)
				.Sum(GetFileLength);
			static long GetFileLength(FileInfo f) => f.Length;
		});
		var size = storageSizeNormalizer.Normalize(sum, out var measure);
		textWriter.WriteLine(
			$"==={Environment.NewLine}Would have removed {processedDirectories.Count} directories, using {size:F2} {measure}.");
	}

	protected override void OnDirectory(DirectoryInfo targetDir)
	{
		if (!targetDir.Exists)
		{
			return;
		}

		processedDirectories.Add(targetDir);
		textWriter.WriteLine($"Would have removed directory {targetDir}");
	}

	protected override void OnStarted()
	{
		processedDirectories.Clear();
	}

	protected override void OnCompleted()
	{
		long sum = processedDirectories.Sum(static e
			=>
		{
			return e.EnumerateFiles("*.*", SearchOption.AllDirectories)
				.Sum(GetFileLength);
			static long GetFileLength(FileInfo f) => f.Length;
		});
		var size = storageSizeNormalizer.Normalize(sum, out var measure);
		textWriter.WriteLine(
			$"==={Environment.NewLine}Would have removed {processedDirectories.Count} directories, using {size:F2} {measure}.");
	}
}

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
