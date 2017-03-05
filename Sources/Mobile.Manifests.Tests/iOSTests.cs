using NUnit.Framework;
using System;
using System.IO;
namespace Mobile.Prerelease.Tests
{
	[TestFixture()]
	public class iOSTest
	{
		const string OriginalManifest = "Manifests/iOS/Info.plist";

		const string Manifest = "Manifests/iOS/Info.Test.plist";

		const string OutputManifest = "Manifests/iOS/Info.Output.plist";

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
			var manifest = new iOSManifest(Manifest);

			Assert.AreEqual("com.test.packaguous", manifest.BundleIdentifier);
			Assert.AreEqual("Packaguous", manifest.DisplayName);
			Assert.AreEqual(new Version(1,0,0,1), manifest.Version);
		}

		[Test()]
		public void Save()
		{
			const string bundleIdentifier = "generatedid";
			const string displayName = "generatedname";
			var version = new Version(1, 2, 3, 4);

			var manifest = new iOSManifest(Manifest);

			Assert.AreNotEqual(bundleIdentifier, manifest.BundleIdentifier);
			Assert.AreNotEqual(displayName, manifest.DisplayName);
			Assert.AreNotEqual(version, manifest.Version);


			manifest.BundleIdentifier = bundleIdentifier;
			manifest.DisplayName = displayName;
			manifest.Version = version;
			manifest.Save(OutputManifest);

			var generated = new iOSManifest(OutputManifest);

			Assert.AreEqual(bundleIdentifier, generated.BundleIdentifier);
			Assert.AreEqual(displayName, generated.DisplayName);
			Assert.AreEqual(version, generated.Version);
		}
	}
}
