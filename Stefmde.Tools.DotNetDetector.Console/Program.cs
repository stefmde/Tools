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
using System.Linq;
using Stefmde.Tools.DotNetDetector.Models;

namespace Stefmde.Tools.DotNetDetector.Console
{
	class Program
	{
		static void Main(string[] args)
		{
			Detector detector = new Detector();
			DotNetVersionResult detectorResult = detector.Detect();
			System.Console.WriteLine(".Net Version Detector");


			System.Console.WriteLine();
			System.Console.WriteLine("Detected .Net Framework Full:");
			PrintVersions(detectorResult.DotNetFrameworkFull);


			System.Console.WriteLine();
			System.Console.WriteLine("Detected .Net SDK/CLR Full:");
			PrintVersions(detectorResult.DotNetSdkFull);


			System.Console.WriteLine();
			System.Console.WriteLine("Detected .Net Framework Core:");
			PrintVersions(detectorResult.DotNetFrameworkCore);


			System.Console.WriteLine();
			System.Console.WriteLine("Detected .Net SDK Core:");
			PrintVersions(detectorResult.DotNetSdkCore);


			System.Console.ReadKey();
		}

		private static void PrintVersions(List<Version> versions)
		{
			if (versions != null && versions.Any())
			{
				foreach (Version version in versions)
				{
					System.Console.WriteLine(version);
				}
			}
			else
			{
				System.Console.WriteLine("None");
			}
		}
	}
}