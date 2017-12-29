namespace Stefmde.Tools.File.MovieInfoReader.Models
{
	public class AudioStream : BaseStream
	{
		public AudioStream(BaseStream baseStream)
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

		public string Profile { get; set; }
		public int Channels { get; set; }
		public string ChannelLayout { get; set; }
		public string SampleFmt { get; set; }
		public int SampleRate { get; set; }
		public int BitsPerRawSample { get; set; }
	}
}
