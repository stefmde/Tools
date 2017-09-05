using System;
using System.Collections.Generic;

namespace Stefmde.DotNetDetector.Models
{
	internal class DotNetVersionMap
	{
		public Version Version { get; set; }

		public List<string> ReleaseKeys { get; set; }
	}
}
