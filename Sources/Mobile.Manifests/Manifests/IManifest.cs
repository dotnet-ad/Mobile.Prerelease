namespace Mobile.Prerelease
{
	using System;
	using System.Collections.Generic;

	public interface IManifest
	{
		#region Properties

		string Path { get; }

		string BundleIdentifier { get; set; }

		string DisplayName { get; set; }

		Version Version { get; set; }

		IEnumerable<string> Icons { get; }

		#endregion

		#region Methods

		void Save(string path);

		#endregion
	}
}
