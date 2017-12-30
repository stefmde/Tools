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

using System.Collections.Generic;

namespace Stefmde.Tools.File.MovieInfoReader.Models
{
	/// <summary>
	/// Global Result object of MovieInfoReader
	/// </summary>
	public class MovieInfo
	{
		public MovieInfo()
		{
			AudioStreams = new List<AudioStream>();
			VideoStreams = new List<VideoStream>();
			SubtitleStreams = new List<SubtitleStream>();
			DataStreams = new List<DataStream>();
			AttachmentStreams = new List<AttachmentStream>();
			Chapters = new List<Chapter>();
		}

		public List<AudioStream> AudioStreams { get; }
		public List<VideoStream> VideoStreams { get; }
		public List<SubtitleStream> SubtitleStreams { get; }
		public List<DataStream> DataStreams { get; }
		public List<AttachmentStream> AttachmentStreams { get; }
		public List<Chapter> Chapters { get; }
	}
}