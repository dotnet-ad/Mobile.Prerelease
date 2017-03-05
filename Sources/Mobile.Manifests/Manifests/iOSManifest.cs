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
			get => GetValue(CFBundleIdentifier);
			set => SetValue(CFBundleIdentifier, value);
		}

		public string DisplayName
		{
			get => GetValue(CFBundleName);
			set
			{
				SetValue(CFBundleName, value);
				SetValue(CFBundleDisplayName, value);
			}
		}

		public Version Version
		{
			get
			{
				var shortVersion = GetValue(CFBundleShortVersionString);
				var bundleVersion = GetValue(CFBundleVersion);

				var shortNumbers = shortVersion.Split('.').Select(x => int.Parse(x));
				var bundleNumbers = bundleVersion.Split('.').Select(x => int.Parse(x));

				return new Version(shortNumbers.ElementAtOrDefault(0), shortNumbers.ElementAtOrDefault(1), shortNumbers.ElementAtOrDefault(2), bundleNumbers.ElementAtOrDefault(0));

			}
			set
			{
				SetValue(CFBundleShortVersionString, $"{value.Major}.{value.Minor}.{value.Build}");
				SetValue(CFBundleVersion, $"{value.Revision}");
			}
		}

		public IEnumerable<string> Icons
		{
			get
			{
				var iconset = GetValue(XSAppIconAssets);
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

		private string GetValue(string key)
		{
			PNode result = null;
			if (this.plist.TryGetValue(key, out result)) return (result as StringNode).Value;
			return null;
		}

		private void SetValue(string key, string value)
		{
			PNode result = null;
			if (this.plist.TryGetValue(key, out result))
			{
				(result as StringNode).Value = value;
				return;
			}
			this.plist[key] = new StringNode(value);
		}

		#endregion
	}
}
