namespace Mobile.Prerelease
{
	using System;
	using PListNet;
	using System.IO;
	using PListNet.Nodes;
	using System.Linq;
	using System.Collections.Generic;

	public class iOSManifest : IManifest
	{
		#region Constructors

		public iOSManifest(string path)
		{
			this.Path = path;
			using (var stream = File.OpenRead(path))
			{
				this.plist = PList.Load(stream) as DictionaryNode;
			}
		}

		#endregion

		#region Keys

		private const string CFBundleIdentifier = "CFBundleIdentifier";

		private const string CFBundleName = "CFBundleName";

		private const string CFBundleDisplayName = "CFBundleDisplayName";

		private const string CFBundleShortVersionString = "CFBundleShortVersionString";

		private const string CFBundleVersion = "CFBundleVersion";

		private const string XSAppIconAssets = "XSAppIconAssets";

		#endregion

		#region Fields

		private DictionaryNode plist;

		#endregion

		#region Properties

		public string BundleIdentifier
		{
			get => (plist[CFBundleIdentifier] as StringNode).Value;
			set => (plist[CFBundleIdentifier] as StringNode).Value = value;
		}

		public string DisplayName
		{
			get => (plist[CFBundleName] as StringNode).Value;
			set
			{
				(plist[CFBundleName] as StringNode).Value = value;
				(plist[CFBundleDisplayName] as StringNode).Value = value;
			}
		}

		public Version Version
		{
			get
			{
				var shortVersion = (plist[CFBundleShortVersionString] as StringNode).Value;
				var bundleVersion = (plist[CFBundleVersion] as StringNode).Value;

				var shortNumbers = shortVersion.Split('.').Select(x => int.Parse(x));
				var bundleNumbers = bundleVersion.Split('.').Select(x => int.Parse(x));

				return new Version(shortNumbers.ElementAtOrDefault(0), shortNumbers.ElementAtOrDefault(1), shortNumbers.ElementAtOrDefault(2), bundleNumbers.ElementAtOrDefault(0));

			}
			set
			{
				(plist[CFBundleShortVersionString] as StringNode).Value = $"{value.Major}.{value.Minor}.{value.Build}";
				(plist[CFBundleVersion] as StringNode).Value = $"{value.Revision}";
			}
		}

		public IEnumerable<string> Icons
		{
			get
			{
				var iconset = (plist[XSAppIconAssets] as StringNode).Value;
				var dir = Directory.GetParent(iconset);
				return dir.GetFiles().Where(f => f.Extension == ".png").Select(x => x.FullName);
			}
		}

		public string Path { get; }

		#endregion

		#region Methods

		public void Save(string path)
		{
			using (var stream = FileHelpers.CreateWithCache(path))
			{
				PList.Save(this.plist, stream, PListFormat.Xml);
			}
		}

		#endregion
	}
}
