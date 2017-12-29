using System.Collections.Generic;

namespace Stefmde.Tools.File.MovieInfoReader.Models
{
	public class MovieInfo
	{
		public MovieInfo()
		{
			AudioStreams = new List<AudioStream>();
			VideoStreams = new List<VideoStream>();
			SubtitleStreams = new List<SubtitleStream>();
			Chapters = new List<Chapter>();
		}

		public List<AudioStream> AudioStreams { get; internal set; }
		public List<VideoStream> VideoStreams { get; internal set; }
		public List<SubtitleStream> SubtitleStreams { get; internal set; }
		public List<Chapter> Chapters { get; set; }
	}
}
