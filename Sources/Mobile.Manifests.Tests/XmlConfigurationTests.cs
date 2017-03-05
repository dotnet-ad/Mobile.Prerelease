using NUnit.Framework;
using System;
using System.IO;
namespace Mobile.Prerelease.Tests
{
	[TestFixture()]
	public class XmlConfigurationTest
	{
		const string OriginalFile = "Configurations/Configuration.xml";

		const string ConfigFile = "Configurations/Configuration.Test.xml";

		const string OutputFile = "Configurations/Configuration.Output.xml";

		[SetUp]
		public void Setup()
		{
			if (File.Exists(ConfigFile))
				File.Delete(ConfigFile);

			File.Copy(OriginalFile, ConfigFile);
		}

		const string path1 = "/Node1/@Attribute1";
		const string path2 = "/Node1/Node2";

		[Test()]
		public void Load()
		{
			var config = new XmlConfiguration(ConfigFile);

			Assert.AreEqual("AValue1", config.Get<string>(path1));
			Assert.AreEqual("NValue2", config.Get<string>(path2));
		}

		[Test()]
		public void Save()
		{

			const string value1 = "NewValue1";
			const string value2 = "NewValue2";

			var config = new XmlConfiguration(ConfigFile);

			Assert.AreEqual("AValue1", config.Get<string>(path1));
			Assert.AreEqual("NValue2", config.Get<string>(path2));

			config.Set(path1, value1);
			config.Set(path2, value2);

			config.Save(OutputFile);

			var generated = new XmlConfiguration(OutputFile);

			Assert.AreEqual(value1, generated.Get<string>(path1));
			Assert.AreEqual(value2, generated.Get<string>(path2));
		}
	}
}
