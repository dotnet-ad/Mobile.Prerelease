namespace Mobile.Prerelease
{
	public interface IConfiguration
	{
		#region Properties

		string Path { get; }

		#endregion

		#region Methods

		void Set<T>(string path, T value);

		T Get<T>(string path);

		void Save(string path);

		#endregion
	}
}
