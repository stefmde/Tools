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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Stefmde.Tools.File.MovieInfoReader.Helper;
using Stefmde.Tools.File.MovieInfoReader.Models;
using Stefmde.Tools.File.MovieInfoReader.Models.CoreModels;
using Stefmde.Tools.File.MovieInfoReader.Models.Enums;

namespace Stefmde.Tools.File.MovieInfoReader
{
	public class MovieInfoReader
	{
		private readonly object _errorLockObject = new object();
		private readonly string _exePath;
		private readonly object _outputLockObject = new object();
		private FileInfo _movieFile;

		/// <summary>
		/// Needed binaries can be found here: https://ffmpeg.zeranoe.com/builds/
		/// Tested with ffmpeg-3.4.1-win64-static
		/// </summary>
		/// <param name="ffprobExePath"></param>
		public MovieInfoReader(string ffprobExePath)
		{
			if (string.IsNullOrEmpty(ffprobExePath))
			{
				throw new ArgumentException("Is not set", nameof(ffprobExePath));
			}

			if (!System.IO.File.Exists(ffprobExePath))
			{
				throw new ArgumentException("File doesn't exists", nameof(ffprobExePath));
			}

			_exePath = ffprobExePath;
		}

		public MovieInfo Read(FileInfo fileInfo)
		{
			_movieFile = fileInfo ?? throw new ArgumentException("Is not set", nameof(fileInfo));

			return ReadInternal();
		}

		public MovieInfo Read(string fullFilePath)
		{
			if (string.IsNullOrEmpty(fullFilePath))
			{
				throw new ArgumentException("Is not set", nameof(fullFilePath));
			}

			if (!System.IO.File.Exists(fullFilePath))
			{
				throw new ArgumentException("File doesn't exists", nameof(fullFilePath));
			}

			_movieFile = new FileInfo(fullFilePath);

			return ReadInternal();
		}


		private MovieInfo ReadInternal()
		{
			string json = InspectFile();

			if (String.IsNullOrEmpty(json) || json.Length <= 10)
			{
				return null;
			}

			CoreRoot core = JsonConvert.DeserializeObject<CoreRoot>(json);

			if (core != null)
			{
				MovieInfo movieInfo = ConvertCoreObject(core);
				return movieInfo;
			}

			return null;
		}

		private string InspectFile()
		{
			string json = "";
			int timeout = 10000;
			string parms = "-show_streams -show_chapters -print_format json -sexagesimal \"" + _movieFile.FullName + "\"";

			// Process source sample: https://stackoverflow.com/a/7608823/7245313
			StringBuilder output = new StringBuilder();
			StringBuilder error = new StringBuilder();

			using (Process process = new Process())
			{
				process.StartInfo.FileName = _exePath;
				process.StartInfo.Arguments = parms;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;

				using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
				using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
				{
					process.OutputDataReceived += (sender, e) =>
					{
						if (e.Data == null)
						{
							outputWaitHandle.Set();
						}
						else
						{
							lock (_outputLockObject)
							{
								output.AppendLine(e.Data);
							}
						}
					};
					process.ErrorDataReceived += (sender, e) =>
					{
						if (e.Data == null)
						{
							errorWaitHandle.Set();
						}
						else
						{
							lock (_errorLockObject)
							{
								error.AppendLine(e.Data);
							}
						}
					};

					process.Start();

					process.BeginOutputReadLine();
					process.BeginErrorReadLine();

					if (process.WaitForExit(timeout) &&
						outputWaitHandle.WaitOne(timeout) &&
						errorWaitHandle.WaitOne(timeout))
					{
						// Process completed. Check process.ExitCode here.
						json = output.ToString();
					}
					else
					{
						// Timed out.
						Console.WriteLine("ERROR timed out on executing ffbrobe: " + error);
					}
				}
			}

			return json;
		}

		private MovieInfo ConvertCoreObject(CoreRoot core)
		{
			MovieInfo movieInfo = new MovieInfo();

			if (core.Streams != null && core.Streams.Any())
			{
				foreach (CoreStream coreStream in core.Streams)
				{
					try
					{
						BaseStream baseStream = GenerateBaseStream(coreStream);

						switch (baseStream.StreamType)
						{
							case StreamType.None:
								break;
							case StreamType.Subtitle:
								SubtitleStream subtitleStream = new SubtitleStream(baseStream);
								subtitleStream.Duration = ParseTimeSpan(coreStream.Duration);
								subtitleStream.DurationTs = coreStream.DurationTs;
								movieInfo.SubtitleStreams.Add(subtitleStream);
								break;
							case StreamType.Audio:
								AudioStream audioStream = GenerateAudioStream(baseStream, coreStream);
								movieInfo.AudioStreams.Add(audioStream);
								break;
							case StreamType.Video:
								VideoStream videoStream = GenerateVideoStream(baseStream, coreStream);
								movieInfo.VideoStreams.Add(videoStream);
								break;
							case StreamType.Data:
								DataStream dataStream = new DataStream();
								movieInfo.DataStreams.Add(dataStream);
								break;
							case StreamType.Attachment:
								AttachmentStream attachmentStream = new AttachmentStream();
								movieInfo.AttachmentStreams.Add(attachmentStream);
								break;
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine("ERROR: Unhandled Exception while parsing streams:");
						Console.WriteLine(ex.Message);
						Console.WriteLine(ex.Source);
						Console.WriteLine(ex.StackTrace);
					}
				}
			}

			if (core.Chapters != null && core.Chapters.Any())
			{
				foreach (CoreChapter coreChapter in core.Chapters)
				{
					try
					{
						Chapter chapter = new Chapter();
						chapter.Id = coreChapter.Id;
						chapter.End = coreChapter.End;
						chapter.Start = coreChapter.Start;
						chapter.TimeBase = coreChapter.TimeBase;
						chapter.Tags = coreChapter.Tags;
						chapter.StartTime = ParseTimeSpan(coreChapter.StartTime);
						chapter.EndTime = ParseTimeSpan(coreChapter.EndTime);

						if (coreChapter.Tags != null && coreChapter.Tags.Any())
						{
							if (coreChapter.Tags.ContainsKey("title"))
							{
								chapter.Title = coreChapter.Tags
									.FirstOrDefault(x => x.Key.Equals("title", StringComparison.OrdinalIgnoreCase)).Value;
							}
						}
						movieInfo.Chapters.Add(chapter);
					}
					catch (Exception ex)
					{
						Console.WriteLine("ERROR: Unhandled Exception while parsing streams:");
						Console.WriteLine(ex.Message);
						Console.WriteLine(ex.Source);
						Console.WriteLine(ex.StackTrace);
					}
				}
			}

			return movieInfo;
		}


		private BaseStream GenerateBaseStream(CoreStream coreStream)
		{
			BaseStream baseStream = new BaseStream();
			baseStream.Index = coreStream.Index;
			baseStream.StartPts = coreStream.StartPts;
			baseStream.Disposition = coreStream.Disposition;
			baseStream.Tags = coreStream.Tags;
			baseStream.TimeBase = coreStream.TimeBase;
			baseStream.StartTime = ParseTimeSpan(coreStream.StartTime);

			if (coreStream.StreamType == null)
			{
				baseStream.StreamType = StreamType.None;
				Console.WriteLine("ERROR: StreamType NULL and not detected!");
			}
			else
			{
				switch (coreStream.StreamType.ToLower())
				{
					case "video":
						baseStream.StreamType = StreamType.Video;
						break;
					case "audio":
						baseStream.StreamType = StreamType.Audio;
						break;
					case "subtitle":
						baseStream.StreamType = StreamType.Subtitle;
						break;
					case "data":
						baseStream.StreamType = StreamType.Data;
						break;
					case "attachment":
						baseStream.StreamType = StreamType.Attachment;
						break;
					default:
						baseStream.StreamType = StreamType.None;
						Console.WriteLine("ERROR: StreamType not detected! Given value: " + coreStream.StreamType.ToLower());
						break;
				}
			}


			if (coreStream.Tags != null && coreStream.Tags.Any())
			{
				if (coreStream.Tags.ContainsKey("language"))
				{
					baseStream.Language = coreStream.Tags
						.FirstOrDefault(x => x.Key.Equals("language", StringComparison.OrdinalIgnoreCase)).Value;
				}
			}

			Codec codec = new Codec();
			codec.TimeBase = coreStream.CodecTimeBase;
			codec.Name = coreStream.CodecName;
			codec.NameLong = coreStream.CodecNameLong;
			codec.Tag = coreStream.CodecTag;
			codec.TagString = coreStream.CodecTagString;
			baseStream.Codec = codec;

			return baseStream;
		}

		private AudioStream GenerateAudioStream(BaseStream baseStream, CoreStream coreStream)
		{
			AudioStream audioStream = new AudioStream(baseStream);
			audioStream.Profile = coreStream.Profile;
			audioStream.Channels = coreStream.Channels;
			audioStream.ChannelLayout = coreStream.ChannelLayout;
			audioStream.SampleFmt = coreStream.SampleFmt;
			audioStream.SampleRate = coreStream.SampleRate;
			audioStream.BitsPerSample = coreStream.BitsPerSample;

			return audioStream;
		}

		private VideoStream GenerateVideoStream(BaseStream baseStream, CoreStream coreStream)
		{
			VideoStream videoStream = new VideoStream(baseStream);
			videoStream.Width = coreStream.Width;
			videoStream.Height = coreStream.Height;
			videoStream.CodedWidth = coreStream.CodedWidth;
			videoStream.CodedHeight = coreStream.CodedHeight;
			videoStream.Profile = coreStream.Profile;
			videoStream.HasBFrames = coreStream.HasBFrames;
			videoStream.PixFmt = coreStream.PixFmt;
			videoStream.Level = coreStream.Level;
			videoStream.Refs = coreStream.Refs;
			videoStream.IsAvc = coreStream.IsAvc;
			videoStream.NalLengthSize = coreStream.NalLengthSize;
			videoStream.BitsPerRawSample = coreStream.BitsPerRawSample;
			videoStream.FieldOrder = coreStream.FieldOrder;
			videoStream.SampleAspectRatio = ParseRatio(coreStream.SampleAspectRatio);
			videoStream.DisplayAspectRatio = ParseRatio(coreStream.DisplayAspectRatio);
			videoStream.RealFrameRate = ParseRational(coreStream.RealFrameRate);
			videoStream.AverageFrameRate = ParseRational(coreStream.AverageFrameRate);


			if (coreStream.ChromaLocation == null)
			{
				videoStream.ChromaLocation = TwoDimensionalOrientation.None;
				Console.WriteLine("Warning: StreamType NULL and not detected!");
			}
			else
			{
				switch (coreStream.ChromaLocation.ToLower())
				{
					case "left":
						videoStream.ChromaLocation = TwoDimensionalOrientation.Left;
						break;
					case "right":
						videoStream.ChromaLocation = TwoDimensionalOrientation.Right;
						break;
					default:
						videoStream.ChromaLocation = TwoDimensionalOrientation.None;
						Console.WriteLine("Warning: StreamType not detected! Given value: " + coreStream.ChromaLocation.ToLower());
						break;
				}
			}

			return videoStream;
		}


		/// <summary>
		/// Parse Ratio out of string
		/// </summary>
		/// <param name="input"></param>
		/// <returns>-1 values on error</returns>
		private Ratio ParseRatio(string input)
		{
			Ratio ratio = new Ratio(-1, -1);

			if (input != null && input.Contains(":") && input.Length >= 3)
			{
				string left = input.Substring(0, input.IndexOf(":"));
				string right = input.Substring(input.IndexOf(":") + 1);
				ratio = new Ratio(Int32.Parse(left), Int32.Parse(right));
			}

			return ratio;
		}

		/// <summary>
		/// Parse Rational out of string
		/// </summary>
		/// <param name="input"></param>
		/// <returns>-1 values on error</returns>
		private Rational ParseRational(string input)
		{
			Rational rational = new Rational(-1, -1);

			if (input != null && input.Contains("/") && input.Length >= 3)
			{
				string left = input.Substring(0, input.IndexOf("/"));
				string right = input.Substring(input.IndexOf("/") + 1);
				rational = new Rational(Int32.Parse(left), Int32.Parse(right));
			}

			return rational;
		}

		private TimeSpan ParseTimeSpan(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return new TimeSpan();
			}

			TimeSpan timeSpan;
			if (TimeSpan.TryParse(input, out timeSpan))
			{
				return timeSpan;
			}

			Console.WriteLine("Warning: can't parse timespan with given value: " + input);
			return new TimeSpan();
		}
	}
}