namespace Mobile.Prerelease
{
	using Newtonsoft.Json.Linq;
	using System.IO;
	using Newtonsoft.Json;

	public class JsonConfiguration : IConfiguration
	{
		public JsonConfiguration(string path)
		{
			this.Path = path;
			using (var stream = File.OpenRead(path))
			using(var reader = new StreamReader(stream))
			using(var jsonReader = new JsonTextReader(reader))
			{
				this.json = JObject.Load(jsonReader);
			}

		}

		private JObject json;

		public string Path { get; }

		public void Save(string path)
		{
			using (var stream = FileHelpers.CreateWithCache(path))
			using (var writer = new StreamWriter(stream))
			using (var jsonWriter = new JsonTextWriter(writer))
			{
				json.WriteTo(jsonWriter);
			}
		}

		public void Set<T>(string path, T value)
		{
			var token = this.json.SelectToken(path) as JValue;
			if(token != null) token.Value = value;
		}

		public T Get<T>(string path)
		{
			var value = this.json.SelectToken(path);
			return value.Value<T>();
		}
	}
}
