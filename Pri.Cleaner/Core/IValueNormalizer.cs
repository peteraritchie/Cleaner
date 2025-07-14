namespace Pri.Cleaner.Core;

/// <summary>
/// A value normalizer
/// </summary>
public interface IValueNormalizer<out TResult, in T>
{
	/// <summary>
	/// Normalize <paramref name="size"/> returning the normalized size and
	/// setting the resulting <paramref name="measure"/>.
	/// </summary>
	/// <returns cref="double">double</returns>
	/// <param name="size" cref="long"></param>
	/// <param name="measure" cref="string"></param>
	TResult Normalize(T size, out string measure);
}
