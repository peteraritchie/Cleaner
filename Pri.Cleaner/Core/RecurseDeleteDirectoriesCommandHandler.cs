using System.Security;

using Microsoft.Extensions.Logging;

namespace Pri.Cleaner.Core;

/// <summary>
/// A RecurseDeleteDirectoriesCommand handler that performs recursive removal of directories.
/// </summary>
public class RecurseDeleteDirectoriesCommandHandler(
	ILogger<RecurseDeleteDirectoriesCommandHandler> logger,
	TextWriter textWriter,
	IValueNormalizer<double, long> storageSizeNormalizer)
	: RecurseDeleteDirectoriesCommandHandlerBase
{
	private readonly Dictionary<DirectoryInfo, long> processedDirectoryInfo = [];

	protected override void OnDirectory(DirectoryInfo directory)
	{
		if (!directory.Exists)
		{
			return;
		}

		try
		{
			logger.LogInformation("Deleting directory {FullName}", directory.FullName);

			directory.Delete(recursive: true);

			var size = directory
				.EnumerateFiles("*.*", SearchOption.AllDirectories)
				.Sum(e => e.Length);
			processedDirectoryInfo[directory] = size;
		}
		catch (UnauthorizedAccessException ex)
		{
			logger.LogError("A readonly file was encountered attempting to remove {FullName}:{NewLine}\t\"{Message}\"",
				directory.FullName,
				Environment.NewLine,
				ex.Message);
		}
		catch (SecurityException ex)
		{
			logger.LogError(ex, "Required permission was missing attempting to remove {FullName}", directory.FullName);
		}
		catch (DirectoryNotFoundException)
		{
			// This can only happen if the file was deleted between enumerating it and trying to delete it,
			// in which case there's nothing to delete, so it ignore it.
			return;
		}
		catch (IOException ex)
		{
			logger.LogError("An error was encountered attempting to remove {FullName}: {Message}",
				directory.FullName,
				ex.Message);
		}
		catch (Exception ex)
		{
			logger.LogError(ex,
				"An error was encountered attempting to remove {FullName}: {Message}",
				directory.FullName,
				ex.Message);
		}
	}
	protected override void OnStarted()
	{
		processedDirectoryInfo.Clear();
	}

	protected override void OnCompleted()
	{
		long totalSize = processedDirectoryInfo.Values.Sum();
		var size = storageSizeNormalizer.Normalize(totalSize, out var measure);
		textWriter.WriteLine(
			$"Removed {processedDirectoryInfo.Count} directories, freeing {size:F2} {measure}.");
	}
}
