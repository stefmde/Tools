using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Stefmde.DotNetDetector.Models;

namespace Stefmde.DotNetDetector.Worker
{
	internal static class Helper
	{
		// ####################################################################################################
		// ### CORE 
		// ####################################################################################################
		internal static List<Version> GetCoreVersionsFromDirectory(string coreVersionDirectory)
		{
			List<Version> versions = new List<Version>();
			//string coreVersionDirectory = @"C:\Program Files\dotnet\shared\Microsoft.NETCore.App"; // SDK: C:\Program Files\dotnet\sdk

			if (Directory.Exists(coreVersionDirectory))
			{
				DirectoryInfo[] directories = new DirectoryInfo(coreVersionDirectory).GetDirectories("*", SearchOption.TopDirectoryOnly);

				foreach (DirectoryInfo directory in directories.Where(x => x.Name.Contains(".")))
				{
					versions.Add(new Version(directory.Name));
				}
			}

			return versions;
		}


		// ####################################################################################################
		// ### FULL 
		// ####################################################################################################

		internal static List<Version> Get1To4FromRegistry()
		{
			List<Version> versions = new List<Version>();
			using (RegistryKey installedVersions = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP"))
			{
				if (installedVersions != null)
				{
					List<string> versionNames = installedVersions.GetSubKeyNames().Where(x => x.StartsWith("v") && x.Contains(".")).Select(x => x.Replace("v", "")).ToList();

					foreach (string versionName in versionNames)
					{
						versions.Add(Version. Parse(versionName));
					}
				}
			}

			return versions;
		}

		internal static Version Get45OrLaterFromRegistry(List<DotNetVersionMap> dotNetVersionMaps)
		{
			using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
			{
				Version version = null;

				if (ndpKey != null)
				{
					string releaseKey =ndpKey.GetValue("Release").ToString();

					foreach (DotNetVersionMap map in dotNetVersionMaps)
					{
						if (map.ReleaseKeys.Contains(releaseKey))
						{
							version = map.Version;
							break;
						}
					}
				}

				return version;
			}
		}
	}
}
