using NUnit.Framework;
using System;
using System.IO;
namespace Mobile.Prerelease.Tests
{
	[TestFixture()]
	public class UwpTest
	{
		const string OriginalManifest = "Manifests/Uwp/Package.appxmanifest";

		const string Manifest = "Manifests/Uwp/Package.Test.appxmanifest";

		const string OutputManifest = "Manifests/Uwp/Package.Output.appxmanifest";

		[SetUp]
		public void Setup()
		{
			if (File.Exists(Manifest))
				File.Delete(Manifest);

			File.Copy(OriginalManifest, Manifest);
		}

		[Test()]
		public void Load()
		{
			var manifest = new UwpManifest(Manifest);

			Assert.AreEqual("com.test.packaguous", manifest.BundleIdentifier);
			Assert.AreEqual("Packaguous", manifest.DisplayName);
			Assert.AreEqual(new Version(1, 0, 0, 1), manifest.Version);
		}

		[Test()]
		public void Save()
		{
			const string bundleIdentifier = "generatedid";
			const string displayName = "generatedname";
			var version = new Version(1, 2, 3, 4);

			var manifest = new UwpManifest(Manifest);

			Assert.AreNotEqual(bundleIdentifier, manifest.BundleIdentifier);
			Assert.AreNotEqual(displayName, manifest.DisplayName);
			Assert.AreNotEqual(version, manifest.Version);


			manifest.BundleIdentifier = bundleIdentifier;
			manifest.DisplayName = displayName;
			manifest.Version = version;
			manifest.Save(OutputManifest);

			var generated = new UwpManifest(OutputManifest);

			Assert.AreEqual(bundleIdentifier, generated.BundleIdentifier);
			Assert.AreEqual(displayName, generated.DisplayName);
			Assert.AreEqual(version, generated.Version);
		}
	}
}
