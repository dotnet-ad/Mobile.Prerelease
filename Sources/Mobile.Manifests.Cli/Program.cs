namespace Mobile.Prerelease.Cli
{
	using System.IO;
	using System;
	using System.Linq;
	using System.Collections.Generic;

	class MainClass
	{
		public static void Main(string[] args)
		{
			string command = null;
			object invokedVerbInstance = null;
			var options = new Options();
			if (!CommandLine.Parser.Default.ParseArgumentsStrict(args, options, (verb, subOptions) =>
				{
					command = verb;
					invokedVerbInstance = subOptions;
				}))
			{
				Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
			}

			switch (command)
			{
				case "manifest":
					UpdateManifest((ManifestSubOptions)invokedVerbInstance);
					break;
				case "configuration":
					UpdateConfiguration((ConfigurationSubOptions)invokedVerbInstance);
					break;
				default:
					Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
					break;
			}
		}

		#region Manifests

		private static readonly string[] IgnoredFolders = { "bin", "obj" };

		private static readonly Dictionary<Platform, string[]> KnownManifests = new Dictionary<Platform, string[]>
		{
			{ Platform.Android, new [] { "androidmanifest.xml" } },
			{ Platform.iOS, new [] { "info.plist" } },
			{ Platform.Uwp, new [] { "package.appxmanifest" } },
		};

		private static void UpdateManifest(ManifestSubOptions options)
		{
			var displayName = options.DisplayName;
			var bundleIdentifier = options.BundleIdentifier;
			var version = options.Version != null ? new Version(options.Version) : null;
			var platform = options.Platform;

			// If a directory is given, then updating all deducted manifests
			if (Directory.Exists(options.File))
			{
				Console.WriteLine($"Searching for potential manifests into {options.File} folder and its subfolders ...");

				var allKnownNames = KnownManifests.SelectMany(x => x.Value);
				var manifests = Directory.GetFiles(options.File, "*", SearchOption.AllDirectories)
										 .Where(x => !IgnoredFolders.Any(id => x.ToLowerInvariant().Replace('\\', '/').IndexOf($"/{id}/") > 0) && allKnownNames.Contains(Path.GetFileName(x.ToLowerInvariant().Replace('\\', '/'))));

				foreach (var file in manifests)
				{
					var mplatform = KnownManifests.First(x => x.Value.Contains(Path.GetFileName(file).ToLowerInvariant())).Key;
					if (platform == Platform.Unknown || platform == mplatform)
					{
						UpdateManifest(file, mplatform, bundleIdentifier, displayName, version);
					}
				}
			}
			else
			{
				UpdateManifest(options.File, platform, bundleIdentifier, displayName, version);
			}
		}

		private static void UpdateManifest(string path, Platform platform, string bundleIdentifier, string displayName, Version version)
		{
			if (platform == Platform.Unknown)
			{
				Console.WriteLine($"Deducing target platform ...");

				switch (Path.GetExtension(path))
				{
					case ".xml":
						platform = Platform.Android;
						break;
					case ".plist":
						platform = Platform.iOS;
						break;
					case ".appxmanifest":
						platform = Platform.Uwp;
						break;
					default:
						throw new MissingMemberException("Platform can't be deducted from file extension");
				}
			}

			Console.WriteLine($"Loading {platform} manifest from {path}");

			IManifest manifest;

			switch (platform)
			{
				case Platform.iOS:
					manifest = new iOSManifest(path);
					break;
				case Platform.Android:
					manifest = new AndroidManifest(path);
					break;
				case Platform.Uwp:
					manifest = new UwpManifest(path);
					break;
				default:
					throw new MissingMemberException("Unknown target platform");
			}

			manifest.DisplayName = displayName ?? manifest.DisplayName;
			manifest.BundleIdentifier = bundleIdentifier ?? manifest.BundleIdentifier;
			manifest.Version = version ?? manifest.Version;

			Console.WriteLine($"Saving {platform} manifest ...");
			manifest.Save(manifest.Path);
			Console.WriteLine($"{platform} manifest succesfully saved to {manifest.Path}");
		}

		#endregion

		#region Configurations

		private static void UpdateConfiguration(ConfigurationSubOptions options)
		{
			if (options.Format == ConfigurationFormat.Unknown)
			{
				Console.WriteLine($"Deducing configuration format ...");

				switch (Path.GetExtension(options.File))
				{
					case ".xml":
						options.Format = ConfigurationFormat.Xml;
						break;
					case ".json":
						options.Format = ConfigurationFormat.Json;
						break;
					default:
						throw new MissingMemberException("Platform can't be deducted from file extension");
				}
			}

			Console.WriteLine($"Loading configuration from {options.File}");

			IConfiguration file;

			switch (options.Format)
			{
				case ConfigurationFormat.Xml:
					file = new XmlConfiguration(options.File);
					break;
				case ConfigurationFormat.Json:
					file = new JsonConfiguration(options.File);
					break;
				default:
					throw new MissingMemberException("Unknown configuration format");
			}

			var values = options.Values.Select(x => x.Split('=')).ToDictionary(x => x.ElementAtOrDefault(0), x => x.ElementAtOrDefault(1));

			foreach (var value in values)
			{
				Console.WriteLine($"Updating value: {value.Key} = {value.Value}");
				file.Set(value.Key, value.Value);
			}

			Console.WriteLine("Saving configuration ...");
			file.Save(file.Path);
			Console.WriteLine($"Configuration succesfully saved to {file.Path}");
		}

		#endregion
	}
}
