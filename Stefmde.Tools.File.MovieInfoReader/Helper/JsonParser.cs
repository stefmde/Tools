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

			foreach (JToken token in movieObject["streams"])
			{
				if (token["codec_type"].ToString().Equals("video", StringComparison.OrdinalIgnoreCase))
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

			foreach (JToken token in movieObject["chapters"])
			{
				Chapter chapter = ParseChapter(token);
				movieInfo.Chapters.Add(chapter);
			}

			return movieInfo;
		}


		private string GetChapterTitle(JToken token)
		{
			string chapterTitle = "";

			if (token["tags"]["title"] != null)
			{
				chapterTitle = token["tags"]["title"].Value<string>();
			}

			return chapterTitle;
		}


		private VideoStream ParseVideoStream(JToken token)
		{
			VideoStream stream = new VideoStream(ParseBaseStream(token))
			{
				Width = token["width"].Value<int>(),
				Height = token["height"].Value<int>(),
				CodedWidth = token["coded_width"].Value<int>(),
				CodedHeight = token["coded_height"].Value<int>(),
				HasBFrames = token["has_b_frames"].Value<bool>(),
				RealFrameRate = ParseRational(token["r_frame_rate"].Value<string>()),
				AverageFrameRate = ParseRational(token["avg_frame_rate"].Value<string>()),
				PixFmt = token["pix_fmt"].Value<string>(),
				ChromaLocation = (TwoDimensionalOrientation) Enum.Parse(typeof(TwoDimensionalOrientation),
					token["chroma_location"].Value<string>(), true),
				Level = token["level"].Value<int>(),
				FieldOrder = token["field_order"].Value<string>(),
				Refs = token["refs"].Value<int>(),
				IsAvc = token["is_avc"].Value<bool>(),
				NalLengthSize = token["nal_length_size"].Value<int>(),
				SampleAspectRatio = ParseRatio(token["sample_aspect_ratio"].Value<string>()),
				DisplayAspectRatio = ParseRatio(token["display_aspect_ratio"].Value<string>()),
				Profile = token["profile"].Value<string>(),
				BitsPerRawSample = token["bits_per_raw_sample"].Value<int>()
			};

			return stream;
		}


		private AudioStream ParseAudioStream(JToken token)
		{
			AudioStream stream = new AudioStream(ParseBaseStream(token))
			{
				Channels = token["channels"].Value<int>(),
				ChannelLayout = token["channel_layout"].Value<string>(),
				SampleFmt = token["sample_fmt"].Value<string>(),
				// TODO Generate Object for it
				SampleRate = token["sample_rate"].Value<int>(),
				Profile = token["profile"].Value<string>(),
				BitsPerRawSample = token["bits_per_raw_sample"].Value<int>()
			};

			return stream;
		}

		private SubtitleStream ParseSubtitleStream(JToken token)
		{
			SubtitleStream stream =
				new SubtitleStream(ParseBaseStream(token))
				{
					Duration = ParseTimeSpan(token["duration"].Value<string>()),
					DurationTs = token["duration_ts"].Value<int>()
				};

			return stream;
		}

		private BaseStream ParseBaseStream(JToken token)
		{
			BaseStream stream = new BaseStream
			{
				TimeBase = token["time_base"].Value<string>(),
				Index = token["index"].Value<int>(),
				StartPts = token["start_pts"].Value<int>(),
				StartTime = TimeSpan.Parse(token["start_time"].Value<string>()),
				Language = token["tags"]["language"].Value<string>(),
				Title = token["tags"]["title"].Value<string>()
			};

			Codec codec = new Codec
			{
				Name = token["codec_name"].Value<string>(),
				NameLong = token["codec_long_name"].Value<string>(),
				Tag = token["codec_tag"].Value<string>(),
				TagString = token["codec_tag_string"].Value<string>(),
				TimeBase = token["codec_time_base"].Value<string>(),
				Type = (StreamType) Enum.Parse(typeof(StreamType), token["codec_type"].Value<string>(), true)
			};
			stream.Codec = codec;

			Disposition disposition = new Disposition
			{
				Defaut = token["disposition"]["default"].Value<int>(),
				Dub = token["disposition"]["dub"].Value<int>(),
				Original = token["disposition"]["original"].Value<int>(),
				Comment = token["disposition"]["comment"].Value<int>(),
				Lyrics = token["disposition"]["lyrics"].Value<int>(),
				Karaoke = token["disposition"]["karaoke"].Value<int>(),
				Forced = token["disposition"]["forced"].Value<int>(),
				HearingImpaired = token["disposition"]["hearing_impaired"].Value<int>(),
				VisualImpaired = token["disposition"]["visual_impaired"].Value<int>(),
				CleanEffects = token["disposition"]["clean_effects"].Value<int>(),
				AttachedPicture = token["disposition"]["attached_pic"].Value<int>(),
				TimedThumbnails = token["disposition"]["timed_thumbnails"].Value<int>()
			};
			stream.Disposition = disposition;

			Tag tag = new Tag();
			TagLanguage tagLanguage = new TagLanguage
			{
				Bps = token["tags"]["BPS"].Value<string>(),
				Duration = ParseTimeSpan(token["tags"]["DURATION"].Value<string>()),
				NumberOfBytes = token["tags"]["NUMBER_OF_BYTES"].Value<long>(),
				NumberOfFrames = token["tags"]["NUMBER_OF_FRAMES"].Value<int>(),
				StatisticsTags = token["tags"]["_STATISTICS_TAGS"].Value<string>().Split(' ').ToList(),
				StatisticsWritingApp = token["tags"]["_STATISTICS_WRITING_APP"].Value<string>(),
				StatisticsWritingUtc = token["tags"]["_STATISTICS_WRITING_DATE_UTC"].Value<DateTime>()
			};
			tag.Tags = tagLanguage;

			TagLanguage tagLanguageEng = new TagLanguage
			{
				Bps = token["tags"]["BPS-eng"].Value<string>(),
				Duration = ParseTimeSpan(token["tags"]["DURATION-eng"].Value<string>()),
				NumberOfBytes = token["tags"]["NUMBER_OF_BYTES-eng"].Value<long>(),
				NumberOfFrames = token["tags"]["NUMBER_OF_FRAMES-eng"].Value<int>(),
				StatisticsTags = token["tags"]["_STATISTICS_TAGS-eng"].Value<string>().Split(' ').ToList(),
				StatisticsWritingApp = token["tags"]["_STATISTICS_WRITING_APP-eng"].Value<string>(),
				StatisticsWritingUtc = token["tags"]["_STATISTICS_WRITING_DATE_UTC-eng"].Value<DateTime>()
			};
			tag.TagsEng = tagLanguageEng;
			stream.Tag = tag;

			return stream;
		}

		private Chapter ParseChapter(JToken token)
		{
			Chapter chapter = new Chapter
			{
				Id = token["id"].Value<int>(),
				TimeBase = token["time_base"].Value<string>(),
				StartTime = ParseTimeSpan(token["start_time"].Value<string>()),
				EndTime = ParseTimeSpan(token["end_time"].Value<string>()),
				Start = token["start"].Value<long>(),
				End = token["end"].Value<long>(),
				Title = GetChapterTitle(token),
				Tags = token["tags"].ToObject<Dictionary<string, string>>()
			};

			return chapter;
		}

		/// <summary>
		/// Helper needed due to incompatible TimeSpan format in json
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private TimeSpan ParseTimeSpan(string input)
		{
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

			if (input.Contains(":") && input.Length >= 3)
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

			if (input.Contains("/") && input.Length >= 3)
			{
				string left = input.Substring(0, input.IndexOf(":"));
				string right = input.Substring(input.IndexOf(":") + 1);
				rational = new Rational(Int32.Parse(left), Int32.Parse(right));
			}

			return rational;
		}
	}
}