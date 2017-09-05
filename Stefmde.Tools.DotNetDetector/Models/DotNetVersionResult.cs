using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stefmde.DotNetDetector.Models
{
	public class DotNetVersionResult
	{
		public DotNetVersionResult()
		{
			DotNetFrameworkFull = new List<Version>();
			DotNetFrameworkCore = new List<Version>();

			DotNetSdkFull = new List<Version>();
			DotNetSdkCore = new List<Version>();
		}

		public List<Version> DotNetFrameworkFull { get; set; }
		public List<Version> DotNetFrameworkCore { get; set; }

		public List<Version> DotNetSdkFull { get; set; }
		public List<Version> DotNetSdkCore { get; set; }

		public bool AnyFrameworkFound => DotNetFrameworkFull.Any() || DotNetFrameworkCore.Any();
		public bool AnySkdFound => DotNetSdkFull.Any() || DotNetSdkCore.Any();
		public bool AnyVersionFound => AnyFrameworkFound || AnySkdFound;
	}
}
