using Stefmde.Tools.File.MovieInfoReader.Models.Enums;

namespace Stefmde.Tools.File.MovieInfoReader.Models
{
	public class Codec
	{
		public string Name { get; set; }
		public string NameLong { get; set; }
		public StreamType Type { get; set; }
		public string TimeBase { get; set; }
		public string Tag { get; set; }
		public string TagString { get; set; }
	}
}
