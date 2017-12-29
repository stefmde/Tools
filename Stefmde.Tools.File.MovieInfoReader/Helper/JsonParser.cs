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

		private Chapter ParseChapter(JToken token)
		{
			Chapter chapter = new Chapter();

			chapter.Id = token["id"].Value<int>();
			chapter.TimeBase = token["time_base"].Value<string>();
			chapter.StartTime = ParseTimeSpan(token["start_time"].Value<string>());
			chapter.EndTime = ParseTimeSpan(token["end_time"].Value<string>());
			chapter.Start = token["start"].Value<long>();
			chapter.End = token["end"].Value<long>();
			chapter.Title = GetChapterTitle(token);
			chapter.Tags = token["tags"].ToObject<Dictionary<string, string>>();

			return chapter;
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
			VideoStream stream = new VideoStream(ParseBaseStream(token));

			stream.Width = token["width"].Value<int>();
			stream.Height = token["height"].Value<int>();
			stream.CodedWidth = token["coded_width"].Value<int>();
			stream.CodedHeight = token["coded_height"].Value<int>();
			stream.HasBFrames = token["has_b_frames"].Value<bool>();
			stream.RealFrameRate = token["r_frame_rate"].Value<string>();
			stream.AverageFrameRate = token["avg_frame_rate"].Value<string>();
			stream.PixFmt = token["pix_fmt"].Value<string>();
			stream.ChromaLocation = (TwoDimensionalOrientation)Enum.Parse(typeof(TwoDimensionalOrientation), token["chroma_location"].Value<string>(), true);
			stream.Level = token["level"].Value<int>();
			stream.FieldOrder = token["field_order"].Value<string>();
			stream.Refs = token["refs"].Value<int>();
			stream.IsAvc = token["is_avc"].Value<bool>();
			stream.NalLengthSize = token["nal_length_size"].Value<int>();
			stream.SampleAspectRatio = ParseRatio(token["sample_aspect_ratio"].Value<string>());
			stream.DisplayAspectRatio = ParseRatio(token["display_aspect_ratio"].Value<string>());
			stream.Profile = token["profile"].Value<string>();
			stream.BitsPerRawSample = token["bits_per_raw_sample"].Value<int>();

			return stream;
		}


		private AudioStream ParseAudioStream(JToken token)
		{
			AudioStream stream = new AudioStream(ParseBaseStream(token));

			stream.Channels = token["channels"].Value<int>();
			// TODO Generate Object for it
			stream.ChannelLayout = token["channel_layout"].Value<string>();
			stream.SampleFmt = token["sample_fmt"].Value<string>();
			stream.SampleRate = token["sample_rate"].Value<int>();
			stream.Profile = token["profile"].Value<string>();
			stream.BitsPerRawSample = token["bits_per_raw_sample"].Value<int>();

			return stream;
		}
		private SubtitleStream ParseSubtitleStream(JToken token)
		{
			SubtitleStream stream = new SubtitleStream(ParseBaseStream(token));

			stream.Duration = ParseTimeSpan(token["duration"].Value<string>());
			stream.DurationTs = token["duration_ts"].Value<int>();

			return stream;
		}

		private BaseStream ParseBaseStream(JToken token)
		{
			BaseStream stream = new BaseStream();

			stream.TimeBase = token["time_base"].Value<string>();

			stream.Index = token["index"].Value<int>();
			stream.StartPts = token["start_pts"].Value<int>();

			stream.StartTime = TimeSpan.Parse(token["start_time"].Value<string>());
			stream.Language = token["tags"]["language"].Value<string>();
			stream.Title = token["tags"]["title"].Value<string>();


			Codec codec = new Codec();
			codec.Name = token["codec_name"].Value<string>();
			codec.NameLong = token["codec_long_name"].Value<string>();
			codec.Tag = token["codec_tag"].Value<string>();
			codec.TagString = token["codec_tag_string"].Value<string>();
			codec.TimeBase = token["codec_time_base"].Value<string>();
			codec.Type = (StreamType)Enum.Parse(typeof(StreamType), token["codec_type"].Value<string>(), true);
			stream.Codec = codec;

			Disposition disposition = new Disposition();
			disposition.Defaut = token["disposition"]["default"].Value<int>();
			disposition.Dub = token["disposition"]["dub"].Value<int>();
			disposition.Original = token["disposition"]["original"].Value<int>();
			disposition.Comment = token["disposition"]["comment"].Value<int>();
			disposition.Lyrics = token["disposition"]["lyrics"].Value<int>();
			disposition.Karaoke = token["disposition"]["karaoke"].Value<int>();
			disposition.Forced = token["disposition"]["forced"].Value<int>();
			disposition.HearingImpaired = token["disposition"]["hearing_impaired"].Value<int>();
			disposition.VisualImpaired = token["disposition"]["visual_impaired"].Value<int>();
			disposition.CleanEffects = token["disposition"]["clean_effects"].Value<int>();
			disposition.AttachedPicture = token["disposition"]["attached_pic"].Value<int>();
			disposition.TimedThumbnails = token["disposition"]["timed_thumbnails"].Value<int>();
			stream.Disposition = disposition;


			Tag tag = new Tag();

			TagLanguage tagLanguage = new TagLanguage();
			tagLanguage.Bps = token["tags"]["BPS"].Value<string>();
			tagLanguage.Duration = ParseTimeSpan(token["tags"]["DURATION"].Value<string>());
			tagLanguage.NumberOfBytes = token["tags"]["NUMBER_OF_BYTES"].Value<long>();
			tagLanguage.NumberOfFrames = token["tags"]["NUMBER_OF_FRAMES"].Value<int>();
			tagLanguage.StatisticsTags = token["tags"]["_STATISTICS_TAGS"].Value<string>().Split(' ').ToList();
			tagLanguage.StatisticsWritingApp = token["tags"]["_STATISTICS_WRITING_APP"].Value<string>();
			tagLanguage.StatisticsWritingUtc = token["tags"]["_STATISTICS_WRITING_DATE_UTC"].Value<DateTime>();
			tag.Tags = tagLanguage;


			TagLanguage tagLanguageEng = new TagLanguage();
			tagLanguageEng.Bps = token["tags"]["BPS-eng"].Value<string>();
			tagLanguageEng.Duration = ParseTimeSpan(token["tags"]["DURATION-eng"].Value<string>());
			tagLanguageEng.NumberOfBytes = token["tags"]["NUMBER_OF_BYTES-eng"].Value<long>();
			tagLanguageEng.NumberOfFrames = token["tags"]["NUMBER_OF_FRAMES-eng"].Value<int>();
			tagLanguageEng.StatisticsTags = token["tags"]["_STATISTICS_TAGS-eng"].Value<string>().Split(' ').ToList();
			tagLanguageEng.StatisticsWritingApp = token["tags"]["_STATISTICS_WRITING_APP-eng"].Value<string>();
			tagLanguageEng.StatisticsWritingUtc = token["tags"]["_STATISTICS_WRITING_DATE_UTC-eng"].Value<DateTime>();
			tag.TagsEng = tagLanguageEng;
			stream.Tag = tag;


			return stream;
		}

		public TimeSpan ParseTimeSpan(string input)
		{
			int dotIndex = input.IndexOf(".");
			if (input.Substring(dotIndex).Length > 7)
			{
				input = input.Substring(0, dotIndex + 7);
			}

			return TimeSpan.Parse(input);
		}

		private Ratio ParseRatio(string input)
		{
			Ratio ratio = new Ratio(0, 0);

			if (input.Contains(":"))
			{
				string left = input.Substring(0, input.IndexOf(":"));
				string right = input.Substring(input.IndexOf(":") + 1);
				ratio = new Ratio(Int32.Parse(left), Int32.Parse(right));
			}

			return ratio;
		}
	}
}
