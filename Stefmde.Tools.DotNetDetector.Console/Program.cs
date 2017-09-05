using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stefmde.DotNetDetector.Models;

namespace Stefmde.DotNetDetector.Console
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
