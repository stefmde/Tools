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
using System.IO;
using System.Linq;
using Microsoft.Win32;
using Stefmde.Tools.DotNetDetector.Models;

namespace Stefmde.Tools.DotNetDetector.Worker
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
				DirectoryInfo[] directories =
					new DirectoryInfo(coreVersionDirectory).GetDirectories("*", SearchOption.TopDirectoryOnly);

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
			using (RegistryKey installedVersions =
				Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP"))
			{
				if (installedVersions != null)
				{
					List<string> versionNames = installedVersions.GetSubKeyNames().Where(x => x.StartsWith("v") && x.Contains("."))
						.Select(x => x.Replace("v", "")).ToList();

					foreach (string versionName in versionNames)
					{
						versions.Add(Version.Parse(versionName));
					}
				}
			}

			return versions;
		}

		internal static Version Get45OrLaterFromRegistry(List<DotNetVersionMap> dotNetVersionMaps)
		{
			using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
				.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
			{
				Version version = null;

				if (ndpKey != null)
				{
					string releaseKey = ndpKey.GetValue("Release").ToString();

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