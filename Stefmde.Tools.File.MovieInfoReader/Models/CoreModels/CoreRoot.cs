using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stefmde.Tools.File.MovieInfoReader.Models.CoreModels
{
	internal class CoreRoot
	{
		[JsonProperty("chapters")]
		internal List<CoreChapter> Chapters { get; set; }

		[JsonProperty("streams")]
		internal List<CoreStream> Streams { get; set; }
	}
}
