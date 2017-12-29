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

namespace Stefmde.Tools.File.Hash.Models
{
	public enum Accuracy
	{
		/// <summary>
		/// Hash first and last 10 KB
		/// </summary> 
		UltraLow = 10240,

		/// <summary>
		/// Hash first and last 100 KB
		/// </summary> 
		Low = 102400,


		/// <summary>
		/// Hash first and last 1 MB
		/// </summary>
		Medium = 1024000,


		/// <summary>
		/// Hash first and last 10 MB
		/// </summary>
		High = 10240000,

		/// <summary>
		/// Hash first and last 100 MB
		/// </summary>
		UltraHigh = 102400000
	}
}