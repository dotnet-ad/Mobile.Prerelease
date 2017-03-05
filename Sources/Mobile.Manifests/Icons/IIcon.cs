namespace Mobile.Prerelease
{
	using System;

	public interface IIcon : IDisposable
	{
		#region Properties

		string Path { get; }

		int Width { get; }

		int Height { get; }

		#endregion

		#region Methods

		void Resize(int width, int height);

		void AnnotateTop(string text, string backgroundColor, string foregroundColor);

		void AnnotateBottom(string text, string backgroundColor, string foregroundColor);

		void Save(string path);

		#endregion
	}
}
