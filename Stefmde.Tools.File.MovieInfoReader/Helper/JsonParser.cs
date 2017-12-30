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
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Stefmde.Tools.File.MovieInfoReader.Extensions;
using Stefmde.Tools.File.MovieInfoReader.Models;
using Stefmde.Tools.File.MovieInfoReader.Models.Enums;

namespace Stefmde.Tools.File.MovieInfoReader.Helper
{
	public class JsonParser
	{
		/// <summary>
		/// Global Json parser manager
		/// </summary>
		/// <param name="json"></param>
		/// <returns></returns>
		internal MovieInfo ReadJson(string json)
		{
			MovieInfo movieInfo = new MovieInfo();
			JObject movieObject = JObject.Parse(json);

			if (movieObject["streams"] == null)
			{
				Console.WriteLine("ERROR: No streams found because collection is null");
			}
			else
			{
				foreach (JToken token in movieObject["streams"])
				{
					try
					{
						if (token["codec_type"] == null)
						{
							Console.WriteLine("ERROR: Stream skipped because its null");
							continue;
						}

						if (token["codec_type"].Value<string>().Equals("video", StringComparison.OrdinalIgnoreCase))
						{
							VideoStream stream = ParseVideoStream(token);
							movieInfo.VideoStreams.Add(stream);
						}
						else if (token["codec_type"].Value<string>().Equals("audio", StringComparison.OrdinalIgnoreCase))
						{
							AudioStream stream = ParseAudioStream(token);
							movieInfo.AudioStreams.Add(stream);
						}
						else if (token["codec_type"].Value<string>().Equals("subtitle", StringComparison.OrdinalIgnoreCase))
						{
							SubtitleStream stream = ParseSubtitleStream(token);
							movieInfo.SubtitleStreams.Add(stream);
						}
						else
						{
							Console.WriteLine("ERROR: Unknown StreamType found: " + token["codec_type"].Value<string>());
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine("ERROR: Unhandled Exception in processing stream");
						Console.WriteLine(ex.Message);
						Console.WriteLine(ex.Source);
						Console.WriteLine(ex.StackTrace);
					}
				}
			}

			if (movieObject["chapters"] == null)
			{
				Console.WriteLine("ERROR: No chapters found because collection is null");
			}
			else
			{
				foreach (JToken token in movieObject["chapters"])
				{
					try
					{
						if (token == null)
						{
							Console.WriteLine("ERROR: Chapter skipped because its null");
							continue;
						}

						Chapter chapter = ParseChapter(token);
						movieInfo.Chapters.Add(chapter);
					}
					catch (Exception ex)
					{
						Console.WriteLine("ERROR: Unhandled Exception in processing chapters");
						Console.WriteLine(ex.Message);
						Console.WriteLine(ex.Source);
						Console.WriteLine(ex.StackTrace);
					}
				}
			}

			return movieInfo;
		}


		


		private VideoStream ParseVideoStream(JToken token)
		{
			VideoStream stream = new VideoStream(ParseBaseStream(token));
			stream.Width = token["width"].Value<int?>() ?? -1;
			stream.Height = token["height"].Value<int?>() ?? -1;
			stream.CodedWidth = token["coded_width"].Value<int?>() ?? -1;
			stream.CodedHeight = token["coded_height"].Value<int?>() ?? -1;
			stream.HasBFrames = token["has_b_frames"].Value<bool?>() ?? false;
			stream.RealFrameRate = ParseRational(token["r_frame_rate"].Value<string>());
			stream.AverageFrameRate = ParseRational(token["avg_frame_rate"].Value<string>());
			stream.PixFmt = token["pix_fmt"].Value<string>();
			stream.ChromaLocation = token["chroma_location"].Value<string>() == null ? TwoDimensionalOrientation.None : (TwoDimensionalOrientation) Enum.Parse(typeof(TwoDimensionalOrientation),
				token["chroma_location"].Value<string>(), true);
			stream.Level = token["level"].Value<int?>() ?? -1;
			stream.FieldOrder = token["field_order"].Value<string>();
			stream.Refs = token["refs"].Value<int?>() ?? -1;
			stream.IsAvc = token["is_avc"].Value<bool?>() ?? false;
			stream.NalLengthSize = token["nal_length_size"].Value<int?>() ?? -1;
			stream.SampleAspectRatio = ParseRatio(token["sample_aspect_ratio"].Value<string>());
			stream.DisplayAspectRatio = ParseRatio(token["display_aspect_ratio"].Value<string>());
			stream.Profile = token["profile"].Value<string>();
			stream.BitsPerRawSample = token["bits_per_raw_sample"].Value<int?>() ?? -1;

			return stream;
		}


		private AudioStream ParseAudioStream(JToken token)
		{
			AudioStream stream = new AudioStream(ParseBaseStream(token));
			stream.Channels = token["channels"].Value<int?>() ?? -1;
			stream.ChannelLayout = token["channel_layout"].Value<string>();
			stream.SampleFmt = token["sample_fmt"].Value<string>();
			stream.SampleRate = token["sample_rate"].Value<int?>() ?? -1;
			stream.Profile = token["profile"].Value<string>();
			stream.BitsPerSample = token["bits_per_raw_sample"].Value<int?>() ?? -1;

			return stream;
		}

		private SubtitleStream ParseSubtitleStream(JToken token)
		{
			SubtitleStream stream =
				new SubtitleStream(ParseBaseStream(token))
				{
					Duration = ParseTimeSpan(token["duration"].Value<string>()),
					DurationTs = token["duration_ts"].Value<int?>() ?? -1
				};

			return stream;
		}

		private BaseStream ParseBaseStream(JToken token)
		{
			BaseStream stream = new BaseStream
			{
				TimeBase = token["time_base"].Value<string>(),
				Index = token["index"].Value<int?>() ?? -1,
				StartPts = token["start_pts"].Value<int?>() ?? -1,
				StartTime = TimeSpan.Parse(token["start_time"].Value<string>()),
				Language = GetItemFromTags(token, "language"),
				Title = GetItemFromTags(token, "title")
			};

			try
			{
				Codec codec = new Codec
				{
					Name = token["codec_name"].Value<string>(),
					NameLong = token["codec_long_name"].Value<string>(),
					Tag = token["codec_tag"].Value<string>(),
					TagString = token["codec_tag_string"].Value<string>(),
					TimeBase = token["codec_time_base"].Value<string>(),
					//Type = token["codec_type"].Value<string>() == null ? StreamType.None : (StreamType)Enum.Parse(typeof(StreamType), token["codec_type"].Value<string>(), true)
				};
				stream.Codec = codec;
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR: Unhandled Exception in processing BaseStream.Codec");
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}

			try
			{
				Disposition disposition = new Disposition();
				disposition.Default = token["disposition"]["default"].Value<int?>() ?? -1;
				disposition.Dub = token["disposition"]["dub"].Value<int?>() ?? -1;
				disposition.Original = token["disposition"]["original"].Value<int?>() ?? -1;
				disposition.Comment = token["disposition"]["comment"].Value<int?>() ?? -1;
				disposition.Lyrics = token["disposition"]["lyrics"].Value<int?>() ?? -1;
				disposition.Karaoke = token["disposition"]["karaoke"].Value<int?>() ?? -1;
				disposition.Forced = token["disposition"]["forced"].Value<int?>() ?? -1;
				disposition.HearingImpaired = token["disposition"]["hearing_impaired"].Value<int?>() ?? -1;
				disposition.VisualImpaired = token["disposition"]["visual_impaired"].Value<int?>() ?? -1;
				disposition.CleanEffects = token["disposition"]["clean_effects"].Value<int?>() ?? -1;
				disposition.AttachedPicture = token["disposition"]["attached_pic"].Value<int?>() ?? -1;
				disposition.TimedThumbnails = token["disposition"]["timed_thumbnails"].Value<int?>() ?? -1;
				//stream.Disposition = disposition;
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR: Unhandled Exception in processing BaseStream.Disposition");
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.Source);
				Console.WriteLine(ex.StackTrace);
			}

			if (token.SelectToken("tags") != null)
			{
				stream.Tags = token["tags"].ToObject<Dictionary<string, string>>();
			}

			//Tag tag = new Tag();
			//TagLanguage tagLanguage = new TagLanguage();
			//tagLanguage.Bps = GetItemFromTags(token, "BPS");
			//tagLanguage.Duration = ParseTimeSpan(GetItemFromTags(token, "DURATION"));
			//tagLanguage.NumberOfBytes = token["tags"]["NUMBER_OF_BYTES"].Value<long?>() ?? -1;
			//tagLanguage.NumberOfFrames = token["tags"]["NUMBER_OF_FRAMES"].Value<int?>() ?? -1;
			//tagLanguage.StatisticsTags = GetItemFromTags(token, "_STATISTICS_TAGS").Split(' ').ToList();
			//tagLanguage.StatisticsWritingApp = GetItemFromTags(token, "_STATISTICS_WRITING_APP");
			//tagLanguage.StatisticsWritingUtc = token["tags"]["_STATISTICS_WRITING_DATE_UTC"].Value<DateTime?>();
			//tag.Tags = tagLanguage;

			//TagLanguage tagLanguageEng = new TagLanguage();
			//tagLanguageEng.Bps = GetItemFromTags(token, "BPS-eng");
			//tagLanguageEng.Duration = ParseTimeSpan(GetItemFromTags(token, "DURATION-eng"));
			//tagLanguageEng.NumberOfBytes = token["tags"]["NUMBER_OF_BYTES-eng"].Value<long?>() ?? -1;
			//tagLanguageEng.NumberOfFrames = token["tags"]["NUMBER_OF_FRAMES-eng"].Value<int?>() ?? -1;
			//tagLanguageEng.StatisticsTags = GetItemFromTags(token, "_STATISTICS_TAGS-eng").Split(' ').ToList();
			//tagLanguageEng.StatisticsWritingApp = GetItemFromTags(token, "_STATISTICS_WRITING_APP-eng");
			//tagLanguageEng.StatisticsWritingUtc = token["tags"]["_STATISTICS_WRITING_DATE_UTC-eng"].Value<DateTime?>();
			//tag.TagsEng = tagLanguageEng;
			//stream.Tag = tag;

			return stream;
		}

		private Chapter ParseChapter(JToken token)
		{
			Chapter chapter = new Chapter();
			chapter.Id = token["id"].Value<int?>() ?? -1;
			chapter.TimeBase = token["time_base"].Value<string>();
			chapter.StartTime = ParseTimeSpan(token["start_time"].Value<string>());
			chapter.EndTime = ParseTimeSpan(token["end_time"].Value<string>());
			chapter.Start = token["start"].Value<long?>() ?? -1;
			chapter.End = token["end"].Value<long?>() ?? -1;
			chapter.Title = GetItemFromTags(token, "title");
			chapter.Tags = token["tags"].ToObject<Dictionary<string, string>>();

			return chapter;
		}


		private string GetItemFromTags(JToken token, string tag)
		{
			string item = "";

			if (token != null && token.SelectToken("tags") != null && token["tags"].SelectToken(tag) != null)
			{
				item = token["tags"][tag].Value<string>();
			}

			return item;
		}



		/// <summary>
		/// Helper needed due to incompatible TimeSpan format in json
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private TimeSpan ParseTimeSpan(string input)
		{
			if (input == null || input.Length < 12)
			{
				return new TimeSpan();
			}

			int dotIndex = input.IndexOf(".");
			if (input.Substring(dotIndex).Length > 7)
			{
				input = input.Substring(0, dotIndex + 7);
			}

			return TimeSpan.Parse(input);
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
				string left = input.Substring(0, input.IndexOf(":"));
				string right = input.Substring(input.IndexOf(":") + 1);
				rational = new Rational(Int32.Parse(left), Int32.Parse(right));
			}

			return rational;
		}
	}
}