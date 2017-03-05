namespace Mobile.Prerelease
{
	using System;
	using System.IO;
	using System.Collections.Generic;
	using System.Xml.Linq;

	public class UwpManifest : IManifest
	{
		#region Constructors

		public UwpManifest(string path)
		{
			this.Path = path;
			using (var stream = File.OpenRead(path))
			{
				this.xml = XElement.Load(stream);
			}
		}

		#endregion

		#region Keys

		private static readonly string[] AttributeTileNames =
		{
			"Image",
			"Square150x150Logo",
			"Square44x44Logo",
			"Wide310x150Logo",
			"Square310x310Logo",
			"Square71x71Logo"
		};

		private const string AttributeName = "Name";

		private const string AttributeVersion = "Version";

		private const string AttributeDisplayName = "DisplayName";

		private const string AttributeDescription = "Description";

		private const string NodeIdentity = "Identity";

		private const string NodeVisualElements = "VisualElements";

		private const string NodeProperties = "Properties";

		private const string NodeDisplayName = "DisplayName";

		private const string NodeApplications = "Applications";

		private const string NodeApplication = "Application";

		private static readonly XNamespace DefaultNamespace = "http://schemas.microsoft.com/appx/manifest/foundation/windows10";

		private static readonly XNamespace UapNamespace = "http://schemas.microsoft.com/appx/manifest/uap/windows10";

		#endregion

		#region Fields

		private XElement xml;

		#endregion

		#region Properties

		public string Path { get; }

		public string BundleIdentifier
		{
			get => xml.Element(DefaultNamespace + NodeIdentity)?.Attribute(AttributeName)?.Value;
			set => xml.Element(DefaultNamespace + NodeIdentity).SetAttributeValue(AttributeName, value);
		}

		public string DisplayName
		{
			get => xml.Element(DefaultNamespace + NodeProperties)?.Element(DefaultNamespace + NodeDisplayName)?.Value;
			set
			{
				xml.Element(DefaultNamespace + NodeProperties).Element(DefaultNamespace + NodeDisplayName).SetValue(value);

				//Tiles
				var apps = xml.Element(DefaultNamespace + NodeApplications).Elements(DefaultNamespace + NodeApplication);
				foreach (var app in apps)
				{
					var visualElement = app.Element(DefaultNamespace + NodeVisualElements);
					visualElement?.SetAttributeValue(AttributeDisplayName, value);
					visualElement?.SetAttributeValue(AttributeDescription, value);
				}
			}
		}

		public Version Version
		{
			get
			{
				var versionName = xml.Element(DefaultNamespace + NodeIdentity)?.Attribute(AttributeVersion)?.Value;
				return Version.Parse(versionName);
			}
			set => xml.Element(DefaultNamespace + NodeIdentity).Attribute(AttributeVersion).SetValue(value.ToString());
		}

		public IEnumerable<string> Icons
		{
			get
			{
				var apps = xml.Element(DefaultNamespace + NodeApplications).Elements(DefaultNamespace + NodeApplication);
				foreach (var app in apps)
				{
					var visualElement = app.Element(NodeVisualElements);
					//TODO
				}
				return null;
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
	}
}
