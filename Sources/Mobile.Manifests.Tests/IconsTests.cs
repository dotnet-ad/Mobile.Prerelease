namespace Mobile.Prerelease.Tests
{
	using NUnit.Framework;
	using System;
	using System.IO;

	[TestFixture()]
	public class IconsTests
	{
		const string OriginalFile = "Icons/Icon.png";

		const string ConfigFile = "Icons/Icon.Test.png";

		const string OutputFile = "Icons/Icon.Output.png";

		[SetUp]
		public void Setup()
		{
			if (File.Exists(ConfigFile))
				File.Delete(ConfigFile);

			File.Copy(OriginalFile, ConfigFile);
		}

		[Test()]
		public void Annotate()
		{
		/*	try
			{
				using (var icon = new Icon(ConfigFile))
				{
					icon.AnnotateTop("Sample", Rgba32.Red, Rgba32.White);
					icon.Save(OutputFile);
				}
			}
			catch (Exception ex)
			{

			}*/
		}
	}
}
