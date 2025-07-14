using Pri.Cleaner;
using Pri.Cleaner.Core;

namespace Tests;

public class NormalizerShould
{
	[Theory]
	[InlineData(1,1, "bytes")]
	[InlineData(1023, 1023, "bytes")]
	[InlineData(1, 1024L, "kilobytes")]
	[InlineData(1, 1024L * 1024, "megabytes")]
	[InlineData(1, 1024L * 1024 * 1024, "gigabytes")]
	[InlineData(1, 1024L * 1024 * 1024 * 1024, "terabytes")]
	[InlineData(1, 1024L * 1024 * 1024 * 1024 * 1024, "petabytes")]
	[InlineData(1024, 1024L * 1024 * 1024 * 1024 * 1024 * 1024, "petabytes")]
	public void NormalizeToMeasure(double expectedNormalizedSize, long size, string expectedMeasure)
	{
		var normalizer = new StorageSizeValueNormalizer();
		Assert.Equal(expectedNormalizedSize, normalizer.Normalize(size, out var measure));
		Assert.Equal(expectedMeasure, measure);
	}
}
