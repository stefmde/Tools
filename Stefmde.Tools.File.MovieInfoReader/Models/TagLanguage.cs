using System;
using System.Collections.Generic;

namespace Stefmde.Tools.File.MovieInfoReader.Models
{
	public class TagLanguage
	{
		public string Bps { get; set; }
		public TimeSpan Duration { get; set; }
		public int NumberOfFrames { get; set; }
		public long NumberOfBytes { get; set; }
		public string StatisticsWritingApp { get; set; }
		public DateTime StatisticsWritingUtc { get; set; }
		public List<string> StatisticsTags { get; set; }
	}
}
