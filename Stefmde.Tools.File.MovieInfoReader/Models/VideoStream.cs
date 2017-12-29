using Stefmde.Tools.File.MovieInfoReader.Models.Enums;

namespace Stefmde.Tools.File.MovieInfoReader.Models
{
	public class VideoStream : BaseStream
	{
		public VideoStream(BaseStream baseStream)
		{
			Index = baseStream.Index;
			Codec = baseStream.Codec;
			StreamType = baseStream.StreamType;
			TimeBase = baseStream.TimeBase;
			StartPts = baseStream.StartPts;
			StartTime = baseStream.StartTime;
			Disposition = baseStream.Disposition;
			Tag = baseStream.Tag;
			Language = baseStream.Language;
			Title = baseStream.Title;
		}

		public int Width { get; set; }
		public int Height { get; set; }
		public int CodedWidth { get; set; }
		public int CodedHeight { get; set; }
		public string Profile { get; set; }
		public bool HasBFrames { get; set; }
		public Ratio SampleAspectRatio { get; set; }
		public Ratio DisplayAspectRatio { get; set; }
		public string RealFrameRate { get; set; }
		public string AverageFrameRate { get; set; }
		public string PixFmt { get; set; }
		public TwoDimensionalOrientation ChromaLocation { get; set; }
		public int Level { get; set; }
		public string FieldOrder { get; set; }
		public int Refs { get; set; }
		public bool IsAvc { get; set; }
		public int NalLengthSize { get; set; }
		public int BitsPerRawSample { get; set; }
	}
}
