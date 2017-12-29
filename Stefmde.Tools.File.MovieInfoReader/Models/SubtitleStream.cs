using System;

namespace Stefmde.Tools.File.MovieInfoReader.Models
{
	public class SubtitleStream : BaseStream
	{
		public SubtitleStream(BaseStream baseStream)
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

		public TimeSpan Duration { get; set; }
		public int DurationTs { get; set; }
	}
}
