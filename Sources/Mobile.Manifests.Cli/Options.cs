namespace Mobile.Prerelease.Cli
{
	using System;
	using CommandLine;

	public enum Platform
	{
		Unknown,
		iOS,
		Android,
		Uwp,
	}

	public enum ConfigurationFormat
	{
		Unknown,
		Json,
		Xml,
	}

	public class ManifestSubOptions
	{
		[Option('f', "File", Required = true, HelpText = "The manifest file path")]
		public string File { get; set; }

		[Option('p', "Platform", HelpText = "The target project platform. If not precised, try to deduct it from file extension.")]
		public Platform Platform { get; set; }

		[Option('v', "Version", HelpText = "The new version")]
		public string Version { get; set; }

		[Option('b', "BundleIdentifier", HelpText = "The new bundle identifier.")]
		public string BundleIdentifier { get; set; }

		[Option('n', "DisplayName", HelpText = "The new display name.")]
		public string DisplayName { get; set; }
	}

	public class ConfigurationSubOptions
	{
		[Option('f', "File", Required = true, HelpText = "The configuration file path")]
		public string File { get; set; }

		[Option('o', "Format", HelpText = "The file format. If not precised, try to deduct it from file extension.")]
		public ConfigurationFormat Format { get; set; }

		[OptionArray('v', "values", DefaultValue = new string[] { })]
		public string[] Values { get; set; }
	}

	public class Options
	{
		[VerbOption("manifest", HelpText = "Updates the given manifest")]
		public ManifestSubOptions ManifestVerb { get; set; }

		[VerbOption("configuration", HelpText = "Updates the given configuration file")]
		public ConfigurationSubOptions ConfigurationVerb { get; set; }
	}
}
