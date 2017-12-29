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

using Stefmde.Tools.File.MovieInfoReader.Models.Enums;

namespace Stefmde.Tools.File.MovieInfoReader.Models
{
	public class VideoStream : BaseStream
	{
		public VideoStream(BaseStream baseStream)
		{
			Index = baseStream.Index;
			Codec = baseStream.Codec;
			StreamType = baseStream.StreamType;
			TimeBase = baseStream.TimeBase;
			StartPts = baseStream.StartPts;
			StartTime = baseStream.StartTime;
			Disposition = baseStream.Disposition;
			Tag = baseStream.Tag;
			Language = baseStream.Language;
			Title = baseStream.Title;
		}

		public int Width { get; internal set; }
		public int Height { get; internal set; }
		public int CodedWidth { get; internal set; }
		public int CodedHeight { get; internal set; }
		public string Profile { get; internal set; }
		public bool HasBFrames { get; internal set; }
		public Ratio SampleAspectRatio { get; internal set; }
		public Ratio DisplayAspectRatio { get; internal set; }
		public Rational RealFrameRate { get; internal set; }
		public Rational AverageFrameRate { get; internal set; }
		public string PixFmt { get; internal set; }
		public TwoDimensionalOrientation ChromaLocation { get; internal set; }
		public int Level { get; internal set; }
		public string FieldOrder { get; internal set; }
		public int Refs { get; internal set; }
		public bool IsAvc { get; internal set; }
		public int NalLengthSize { get; internal set; }
		public int BitsPerRawSample { get; internal set; }
	}
}