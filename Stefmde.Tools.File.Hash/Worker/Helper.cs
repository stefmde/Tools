using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Stefmde.File.Hash.Models;

namespace Stefmde.File.Hash.Worker
{
	internal class Helper
	{
		internal static HashResult DoHash(FileInfo fileInfo, int bytes)
		{
			DateTime dtStart = DateTime.Now;
			HashResult result = new HashResult();
			result.FileInfo = fileInfo;
			result.HashedBytes = bytes;

			try
			{
				byte[] buffer = ReadFirstFileBytes(fileInfo.FullName, bytes);
				result.HashBeginning = ComputeHash(buffer);


				buffer = ReadLastFileBytes(fileInfo.FullName, bytes);
				result.HashEnding = ComputeHash(buffer);

				result.Success = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}

			result.Duration = DateTime.Now - dtStart;
			return result;
		}


		internal static string ComputeHash(byte[] data)
		{
			string hash;
			using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
			{
				hash = Convert.ToBase64String(sha1.ComputeHash(data));
			}
			return hash;
		}

		internal static byte[] ReadLastFileBytes(string fileName, int byteCount = 1024)
		{
			byte[] buffer = new byte[byteCount];

			using (BinaryReader reader = new BinaryReader(new FileStream(fileName, FileMode.Open)))
			{
				reader.BaseStream.Position = reader.BaseStream.Length - byteCount;
				reader.BaseStream.Read(buffer, 0, byteCount);
			}

			return buffer;
		}

		internal static byte[] ReadFirstFileBytes(string fileName, int byteCount = 1024)
		{
			byte[] buffer = new byte[byteCount];

			using (BinaryReader reader = new BinaryReader(new FileStream(fileName, FileMode.Open)))
			{
				reader.BaseStream.Position = 0;
				reader.BaseStream.Read(buffer, 0, byteCount);
			}

			return buffer;
		}
	}
}
