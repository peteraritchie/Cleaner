namespace Pri.Cleaner.Core;

/// <summary>
/// An RecurseDeleteDirectoriesCommand handler abstraction
/// </summary>
public interface IRecurseDeleteDirectoriesCommandHandler
{
	/// <summary>
	/// Process a RecurseDeleteDirectoriesCommand.
	/// </summary>
	/// <param name="command"></param>
	void Execute(RecurseDeleteDirectoriesCommand command);
}
