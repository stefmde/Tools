using System;
using System.Collections.Generic;

namespace Stefmde.Tools.File.MovieInfoReader.Models
{
	public class Chapter
	{
		public int Id { get; set; }
		public string TimeBase { get; set; }
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public long Start { get; set; }
		public long End { get; set; }
		public string Title { get; set; }
		public Dictionary<string, string> Tags { get; set; }
	}
}
