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

using System.IO;
using Stefmde.Tools.File.Hash.Models;
using Stefmde.Tools.File.Hash.Worker;

namespace Stefmde.Tools.File.Hash
{
	/// <summary>
	/// This Hasher is designed to run as fast as possible with the lowest collision rate possible
	/// </summary>
	public static class Hash
	{
		/// <summary>
		/// Extension for FileInfo to compute the hash. Only FileInfo supported because of the need of the data in it
		/// </summary>
		/// <param name="fileInfo">File to hash</param>
		/// <param name="accuracy">Accuracy of the hash. More exact = slower</param>
		/// <returns></returns>
		public static HashResult HashFile(this FileInfo fileInfo, Accuracy accuracy = Accuracy.Medium)
		{
			int bytes;

			if (!System.IO.File.Exists(fileInfo.FullName))
			{
				return new HashResult() {FileInfo = fileInfo};
			}

			if (fileInfo.Length < (int) accuracy * 2)
			{
				bytes = (int) fileInfo.Length / 2;
			}
			else
			{
				bytes = (int) accuracy;
			}

			return Helper.ComputeHash(fileInfo, bytes);
		}
	}
}