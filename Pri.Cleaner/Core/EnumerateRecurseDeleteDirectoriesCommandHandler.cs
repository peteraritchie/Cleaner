using Microsoft.Extensions.Logging;

namespace Pri.Cleaner.Core;

/// <summary>
/// A dry-run RecurseDeleteDirectoriesCommand handler
/// </summary>
public class EnumerateRecurseDeleteDirectoriesCommandHandler(
	TextWriter textWriter,
	IValueNormalizer<double, long> storageSizeNormalizer)
	: RecurseDeleteDirectoriesCommandHandlerBase
{
	private readonly List<DirectoryInfo> processedDirectories = [];

	protected override void OnDirectory(DirectoryInfo directory)
	{
		if (!directory.Exists)
		{
			return;
		}

		processedDirectories.Add(directory);
		textWriter.WriteLine($"Would have removed directory {directory}");
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
