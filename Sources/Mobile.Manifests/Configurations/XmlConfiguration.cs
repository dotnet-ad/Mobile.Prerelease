namespace Mobile.Prerelease
{
	using System.IO;
	using System.Xml.XPath;
	using System.Xml.Linq;
	using System.Linq;
	using System.Collections;
	using System;

	public class XmlConfiguration : IConfiguration
	{
		public XmlConfiguration(string path)
		{
			this.Path = path;

			using (var stream = File.OpenRead(path))
			{
				this.xml = XDocument.Load(stream);
			}
		}

		private XDocument xml;

		public string Path { get; }

		public void Save(string path)
		{
			using (var stream = FileHelpers.CreateWithCache(path))
			{
				xml.Save(stream);
			}
		}

		public void Set<T>(string path, T value)
		{
			var token = ((IEnumerable)xml.XPathEvaluate(path)).Cast<XObject>().FirstOrDefault();

			switch (token)
			{
				case XElement element :
					element.SetValue(value);
					break;
				case XAttribute attribute:
					attribute.SetValue(value);
					break;
			}
		}

		public T Get<T>(string path)
		{
			var token = ((IEnumerable)xml.XPathEvaluate(path)).Cast<XObject>().FirstOrDefault();
			switch (token)
			{
				case XElement element:
					return (T)Convert.ChangeType(element.Value, typeof(T));
				case XAttribute attribute:
					return (T)Convert.ChangeType(attribute.Value, typeof(T));
			}

			return default(T);
		}
	}
}
