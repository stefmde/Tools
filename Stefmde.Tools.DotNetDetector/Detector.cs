// 
// MIT License
// 
// Copyright(c) 2017 - 2017
// Stefan Müller, Stefm, https://Stefm.de, https://github.com/stefmde/Tools
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 

using System;
using System.Collections.Generic;
using Stefmde.Tools.DotNetDetector.Models;
using Stefmde.Tools.DotNetDetector.Worker;

namespace Stefmde.Tools.DotNetDetector
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
				DotNetSdkFull = new List<Version>() {Environment.Version}
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