namespace Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
		var path = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "src");
		var fs = Directory.EnumerateFiles(path, "*.sln", SearchOption.AllDirectories);
		foreach (var f in fs)
		{
			var dir = Path.GetDirectoryName(f);
			if (dir != null && Directory.Exists(Path.Combine(dir, "obj")))
			{
				Assert.False(true);
			}
		}
    }
}
