﻿// 
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

namespace Stefmde.Tools.File.MovieInfoReader.Models
{
	public class SubtitleStream : BaseStream
	{
		public SubtitleStream(BaseStream baseStream)
		{
			Index = baseStream.Index;
			Codec = baseStream.Codec;
			StreamType = baseStream.StreamType;
			TimeBase = baseStream.TimeBase;
			StartPts = baseStream.StartPts;
			StartTime = baseStream.StartTime;
			Disposition = baseStream.Disposition;
			Tags = baseStream.Tags;
			Language = baseStream.Language;
			Title = baseStream.Title;
		}

		public TimeSpan Duration { get; internal set; }
		public int DurationTs { get; internal set; }
	}
}