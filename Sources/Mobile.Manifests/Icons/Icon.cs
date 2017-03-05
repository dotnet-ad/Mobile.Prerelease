namespace Mobile.Prerelease
{
	using System;
	using NGraphics;

	public class Icon : IIcon
	{
		public Icon(string path, IPlatform platform)
		{
			this.Path = path;

			var img = platform.LoadImage(path);
			this.image = platform.CreateImageCanvas(img.Size);
			this.image.DrawImage(img, new Rect(Point.Zero, this.image.Size));
		}

		private IImageCanvas image;

		public string Path { get; }

		public int Width => (int)this.image.Size.Width;

		public int Height => (int)this.image.Size.Height;

		public void AnnotateTop(string text, string backgroundColor, string foregroundColor)
		{
			var height = this.Height / 10;
			this.image.FillRectangle(0, 0, this.Width, height, new Color(backgroundColor));
			//TODO
		}

		public void AnnotateBottom(string text, string backgroundColor, string foregroundColor)
		{
			var height = this.Height / 10;
			this.image.FillRectangle(0, this.Height - height, this.Width, height, new Color(backgroundColor));
			//TODO
		}

		public void Save(string path)
		{
			this.image.GetImage().SaveAsPng(path);
		}

		public void Dispose() { }

		public void Resize(int width, int height)
		{
			this.image.Scale(width, height);
		}
	}
}
