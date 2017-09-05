using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using Stefmde.DotNetDetector.Models;
using Stefmde.DotNetDetector.Worker;

namespace Stefmde.DotNetDetector
{
	// https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed
	public class Detector
	{
		private readonly List<DotNetVersionMap> _dotNetVersionMaps;
		public Detector()
		{
			_dotNetVersionMaps = MapConfigurationReader.Read("DotNetVersionMap.json");
		}

		public DotNetVersionResult Detect()
		{
			DotNetVersionResult result = new DotNetVersionResult
			{
				DotNetFrameworkCore = Helper.GetCoreVersionsFromDirectory(@"C:\Program Files\dotnet\shared\Microsoft.NETCore.App"),
				DotNetSdkCore = Helper.GetCoreVersionsFromDirectory(@"C:\Program Files\dotnet\sdk"),
				DotNetFrameworkFull = Helper.Get1To4FromRegistry(),
				DotNetSdkFull = new List<Version>() { Environment.Version }
			};

			Version latestFullFrameworkVersion = Helper.Get45OrLaterFromRegistry(_dotNetVersionMaps);
			if (latestFullFrameworkVersion != null)
			{
				result.DotNetFrameworkFull.Add(latestFullFrameworkVersion);
			}

			return result;
		}
	}
}
