namespace Mobile.Prerelease
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Collections.Generic;
	using System.Xml.Linq;

	public class AndroidManifest : IManifest
	{
		#region Constructors

		public AndroidManifest(string path)
		{
			this.Path = path;
			using (var stream = File.OpenRead(path))
			{
				this.xml = XElement.Load(stream);
			}
		}

		#endregion

		#region Keys

		private const string AttributePackage = "package";

		private const string AttributeLabel = "label";

		private const string AttributeIcon = "icon";

		private const string AttributeVersionName = "versionName";

		private const string AttributeVersionCode = "versionCode";

		private const string NodeApplication = "application";

		private static readonly XNamespace AndroidNamespace = "http://schemas.android.com/apk/res/android";

		#endregion

		#region Fields

		private XElement xml;

		#endregion

		#region Properties

		public string Path { get; }

		public string BundleIdentifier
		{
			get => xml.Attribute(AttributePackage).Value;
			set => xml.SetAttributeValue(AttributePackage, value);
		}

		public string DisplayName
		{
			get => xml.Element(NodeApplication).Attribute(AndroidNamespace + AttributeLabel).Value;
			set => xml.Element(NodeApplication).SetAttributeValue(AndroidNamespace + AttributeLabel, value);
		}

		public Version Version
		{
			get
			{
				var versionName = xml.Attribute(AndroidNamespace + AttributeVersionName).Value;
				var versionCode = xml.Attribute(AndroidNamespace + AttributeVersionCode).Value;

				var shortNumbers = versionName.Split('.').Select(x => int.Parse(x));
				var bundleNumbers = versionCode.Split('.').Select(x => int.Parse(x));

				return new Version(shortNumbers.ElementAtOrDefault(0), shortNumbers.ElementAtOrDefault(1), shortNumbers.ElementAtOrDefault(2), bundleNumbers.ElementAtOrDefault(0));

			}
			set
			{
				xml.SetAttributeValue(AndroidNamespace + AttributeVersionName, $"{value.Major}.{value.Minor}.{value.Build}");
				xml.SetAttributeValue(AndroidNamespace + AttributeVersionCode, $"{value.Revision}");
			}
		}

		public IEnumerable<string> Icons
		{
			get
			{
				var iconName = xml.Element(NodeApplication).Attribute(AndroidNamespace + AttributeIcon).Value;
				return this.ResolveIcons(iconName);
			}
		}

		public bool IsXamarinProject
		{
			get
			{
				var dir = Directory.GetParent(this.Path);
				return dir.GetFiles().Any(x => x.Extension == "csproj");
			}
		}

		public string ResourceFolder
		{
			get
			{
				var dir = Directory.GetParent(this.Path);

				if (this.IsXamarinProject)
				{
					return System.IO.Path.Combine(dir.FullName, "Resources");
				}

				return System.IO.Path.Combine(dir.FullName, "Resources");
			}
		}

		#endregion

		#region Methods

		public void Save(string path)
		{
			using (var stream = FileHelpers.CreateWithCache(path))
			{
				this.xml.Save(stream);
			}
		}

		#endregion

		#region Private methods

		private IEnumerable<string> ResolveIcons(string resource)
		{
			if (resource?.StartsWith("@") ?? false)
			{
				var parts = resource.Substring(1);
				var subfoldername = resource.ElementAtOrDefault(0);
				var resname = resource.ElementAtOrDefault(1);
				var resfolder = this.ResourceFolder;
				var resSubfolders = Directory.GetDirectories(ResourceFolder);
			}

			return null;
		}

		#endregion
	}
}
