using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Stefmde.DotNetDetector.Models;

namespace Stefmde.DotNetDetector.Worker
{
	internal static class MapConfigurationReader
	{
		public static List<DotNetVersionMap> Read(string fileName)
		{
			List<DotNetVersionMap> maps = new List<DotNetVersionMap>();

			if (File.Exists(fileName))
			{
				string jsonContent = File.ReadAllText(fileName);
				maps = JsonConvert.DeserializeObject<List<DotNetVersionMap>>(jsonContent);
			}

			return maps;
		}
	}
}
