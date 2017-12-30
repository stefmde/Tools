using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stefmde.Tools.File.MovieInfoReader.Models.CoreModels
{
	internal class CoreChapter
	{
		[JsonProperty("id")]
		public int Id { get; internal set; }

		[JsonProperty("time_base")]
		public string TimeBase { get; internal set; }

		[JsonProperty("start_time")]
		public string StartTime { get; internal set; }

		[JsonProperty("end_time")]
		public string EndTime { get; internal set; }

		[JsonProperty("start")]
		public long Start { get; internal set; }

		[JsonProperty("end")]
		public long End { get; internal set; }

		[JsonProperty("tags")]
		public Dictionary<string, string> Tags { get; internal set; }
	}
}
