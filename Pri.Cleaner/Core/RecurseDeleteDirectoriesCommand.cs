using System.Diagnostics.CodeAnalysis;

namespace Pri.Cleaner.Core;

/// <summary>
/// A command abstraction to encapsulate intent to recurse directories to delete files.
/// </summary>
public sealed class RecurseDeleteDirectoriesCommand(params string[] paths)
{
	/// <summary>
	/// A collection of paths to recurse
	/// </summary>
	[NotNull]
	public readonly IReadOnlyCollection<string>? Paths = paths;
}
