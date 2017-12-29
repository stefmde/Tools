using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stefmde.Tools.File.ConfigurationManager
{
	public class XmlConfigurationManager<T> : IBaseConfigurationManager<T> where T : new()
	{
		public T ReadConfiguration(string path)
		{
			throw new NotImplementedException("It's currently not implemented. Maybe it will done in the future.");
		}

		public bool WriteConfiguration(string path, T data)
		{
			throw new NotImplementedException("It's currently not implemented. Maybe it will done in the future.");
		}
	}
}
