using System;
using Stefmde.Tools.File.MovieInfoReader.Models.Enums;

namespace Stefmde.Tools.File.MovieInfoReader.Models
{
	public class BaseStream
	{
		public int Index { get; set; }
		public string Language { get; set; }
		public string Title { get; set; }
		public Codec Codec { get; set; }
		public StreamType StreamType { get; set; }
		public string TimeBase { get; set; }
		public int StartPts { get; set; }
		public TimeSpan StartTime { get; set; }
		public Disposition Disposition { get; set; }
		public Tag Tag { get; set; }
	}
}
