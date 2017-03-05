using NUnit.Framework;
using System;
using System.IO;
namespace Mobile.Prerelease.Tests
{
	[TestFixture()]
	public class JsonConfigurationTest
	{
		const string OriginalFile = "Configurations/Configuration.json";

		const string ConfigFile = "Configurations/Configuration.Test.json";

		const string OutputFile = "Configurations/Configuration.Output.json";

		[SetUp]
		public void Setup()
		{
			if (File.Exists(ConfigFile))
				File.Delete(ConfigFile);

			File.Copy(OriginalFile, ConfigFile);
		}

		[Test()]
		public void Load()
		{
			var config = new JsonConfiguration(ConfigFile);

			Assert.AreEqual("Value1", config.Get<string>("Key1"));
			Assert.AreEqual("Value2", config.Get<string>("Key2"));
		}

		[Test()]
		public void Save()
		{
			const string value1 = "NewValue1";
			const string value2 = "NewValue2";

			var config = new JsonConfiguration(ConfigFile);

			Assert.AreNotEqual(value1, config.Get<string>("Key1"));
			Assert.AreNotEqual(value2, config.Get<string>("Key2"));

			config.Set("Key1", value1);
			config.Set("Key2", value2);

			config.Save(OutputFile);

			var generated = new JsonConfiguration(OutputFile);

			Assert.AreEqual(value1, generated.Get<string>("Key1"));
			Assert.AreEqual(value2, generated.Get<string>("Key2"));
		}
	}
}
