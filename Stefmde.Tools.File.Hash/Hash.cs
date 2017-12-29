using System.IO;
using Stefmde.File.Hash.Models;
using Stefmde.File.Hash.Worker;

namespace Stefmde.Tools.File.Hash
{
	public static class Hash
	{
		public static HashResult HashFile(this FileInfo fileInfo, Accuracy accuracy = Accuracy.Medium)
		{
			int bytes;

			if (!System.IO.File.Exists(fileInfo.FullName))
			{
				return new HashResult(){FileInfo = fileInfo};
			}

			if (fileInfo.Length < (int)accuracy * 2)
			{
				bytes = (int)fileInfo.Length / 2;
			}
			else
			{
				bytes = (int)accuracy;
			}

			return Helper.DoHash(fileInfo, bytes);
		}
	}
}
