using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stefmde.Tools.File.MovieInfoReader.Extensions
{
	public static class JTokenExtensions
	{
		// Sample: https://stackoverflow.com/a/9591642/7245313
		public static T GetValue<T>(this JToken jToken, string key, T defaultValue = default(T))
		{
			object ret = null;

			try
			{
				ret = jToken.SelectToken(key);
				if (ret == null)
				{
					return defaultValue;
				}

				if (ret is JObject)
				{
					return JsonConvert.DeserializeObject<T>(ret.ToString());
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR: Unhandled Exception while getting value from JToken with extension");
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}
			
			return (T)ret;
		}
	}
}
