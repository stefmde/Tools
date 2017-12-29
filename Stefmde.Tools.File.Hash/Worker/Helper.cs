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
using System.IO;
using System.Security.Cryptography;
using Stefmde.Tools.File.Hash.Models;

namespace Stefmde.Tools.File.Hash.Worker
{
	internal static class Helper
	{
		internal static HashResult ComputeHash(FileInfo fileInfo, int bytes)
		{
			DateTime dtStart = DateTime.Now;
			HashResult result = new HashResult
			{
				FileInfo = fileInfo,
				HashedBytes = bytes
			};

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


		private static string ComputeHash(byte[] data)
		{
			string hash;
			using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
			{
				hash = Convert.ToBase64String(sha1.ComputeHash(data));
			}

			return hash;
		}

		private static byte[] ReadLastFileBytes(string fileName, int byteCount = 1024)
		{
			byte[] buffer = new byte[byteCount];

			using (BinaryReader reader = new BinaryReader(new FileStream(fileName, FileMode.Open)))
			{
				reader.BaseStream.Position = reader.BaseStream.Length - byteCount;
				reader.BaseStream.Read(buffer, 0, byteCount);
			}

			return buffer;
		}

		private static byte[] ReadFirstFileBytes(string fileName, int byteCount = 1024)
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