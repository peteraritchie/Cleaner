namespace Pri.Cleaner.Core;

/// <summary>
/// A utility class that normalizes storage size (bytes) to one of
/// bytes, kilobytes, megabytes, gigabytes, petabytes
/// </summary>
public class StorageSizeValueNormalizer : IValueNormalizer<double, long>
{
	private static readonly string[] Measures = ["bytes", "kilobytes", "megabytes", "gigabytes", "terabytes", "petabytes"];

	/// <summary>
	/// Normalize <paramref name="size"/> returning the normalized size and
	/// setting the resulting <paramref name="measure"/>.
	/// </summary>
	/// <returns cref="double">double</returns>
	/// <param name="size" cref="long"></param>
	/// <param name="measure" cref="string"></param>
	public double Normalize(long size, out string measure)
	{
		double result = size;
		int index = 0;
		while (result >= 1024)
		{
			result /= 1024;
			index++;
			if (index == Measures.Length-1) break;
		}
		measure = Measures[index];
		return result;
	}
}
