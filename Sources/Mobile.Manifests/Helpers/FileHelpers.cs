namespace Mobile.Prerelease
{
	using System.IO;

	public static class FileHelpers
	{
		public static Stream CreateWithCache(string path)
		{
			if (File.Exists(path))
			{
				var cache = path + ".cache";
				if (File.Exists(cache))
				{
					File.Delete(cache);
				}

				File.Copy(path, cache);
				File.Delete(path);
			}

			return File.Create(path);
		}
	}
}
