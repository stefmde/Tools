using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stefmde.Tools.File.ConfigurationManager
{
	public class JsonConfigurationManager<T> : IBaseConfigurationManager<T> where T : new()
	{
		/// <summary>
		/// Reads the configuration from file
		/// </summary>
		/// <param name="path">Can be a path where the class-name is used as filename or a filename</param>
		/// <returns>The object who is generated from file. New object on error. Never null</returns>
		public T ReadConfiguration(string path)
		{
			try
			{
				string fullFileName = path;
				if (System.IO.File.GetAttributes(path).HasFlag(FileAttributes.Directory))
				{
					// Directory given
					fullFileName = Path.Combine(path, typeof(T).Name + ".json");
				}

				string json = System.IO.File.ReadAllText(fullFileName);
				T data = JsonConvert.DeserializeObject<T> (json);

				return data;
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR: " + nameof(JsonConfigurationManager<T>) + " while trying to serialide/write configuration:");
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}

			return new T();
		}

		/// <summary>
		/// Writes the configuration to file
		/// </summary>
		/// <param name="path">Can be a path where the class-name is used as filename or a filename</param>
		/// <param name="data">The object who have to be writen to the file</param>
		/// <returns>True if successfully serialized and written to file</returns>
		public bool WriteConfiguration(string path, T data)
		{
			try
			{
				string fullFileName = path;
				if (System.IO.File.GetAttributes(path).HasFlag(FileAttributes.Directory))
				{
					// Directory given
					fullFileName = Path.Combine(path, typeof(T).Name + ".json");
				}

				string json = JsonConvert.SerializeObject(data, Formatting.Indented);
				System.IO.File.WriteAllText(fullFileName, json);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR: " + nameof(JsonConfigurationManager<T>) + " whil trying to serialide/write configuration:");
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}

			return false;
		}
	}
}
