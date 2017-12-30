using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stefmde.Tools.File.MovieInfoReader.Models.Enums;

namespace Stefmde.Tools.File.MovieInfoReader.Models.CoreModels
{
	class CoreStream
	{
		[JsonProperty("index")]
		public int Index { get; set; }

		[JsonProperty("codec_type")]
		public string StreamType { get; set; }

		[JsonProperty("time_base")]
		public string TimeBase { get; set; }

		[JsonProperty("start_pts")]
		public int StartPts { get; set; }

		[JsonProperty("start_time")]
		public string StartTime { get; set; }

		[JsonProperty("disposition")]
		public Dictionary<string, string> Disposition { get; set; }

		[JsonProperty("tags")]
		public Dictionary<string, string> Tags { get; set; }


		[JsonProperty("profile")]
		public string Profile { get; set; }

		[JsonProperty("channels")]
		public int Channels { get; set; }

		[JsonProperty("channel_layout")]
		public string ChannelLayout { get; set; }

		[JsonProperty("sample_fmt")]
		public string SampleFmt { get; set; }

		[JsonProperty("sample_rate")]
		public int SampleRate { get; set; }

		[JsonProperty("bits_per_raw_sample")]
		public int BitsPerRawSample { get; set; }

		[JsonProperty("bits_per_sample")]
		public int BitsPerSample { get; set; }

		[JsonProperty("duration")]
		public string Duration { get; set; }

		[JsonProperty("duration_ts")]
		public int DurationTs { get; set; }

		[JsonProperty("width")]
		public int Width { get; set; }

		[JsonProperty("height")]
		public int Height { get; set; }

		[JsonProperty("coded_width")]
		public int CodedWidth { get; set; }

		[JsonProperty("coded_height")]
		public int CodedHeight { get; set; }

		[JsonProperty("has_b_frames")]
		public bool HasBFrames { get; set; }

		[JsonProperty("sample_aspect_ratio")]
		public string SampleAspectRatio { get; set; }

		[JsonProperty("display_aspect_ratio")]
		public string DisplayAspectRatio { get; set; }

		[JsonProperty("r_frame_rate")]
		public string RealFrameRate { get; set; }

		[JsonProperty("avg_frame_rate")]
		public string AverageFrameRate { get; set; }

		[JsonProperty("pix_fmt")]
		public string PixFmt { get; set; }

		[JsonProperty("chroma_location")]
		public string ChromaLocation { get; set; }

		[JsonProperty("level")]
		public int Level { get; set; }

		[JsonProperty("field_order")]
		public string FieldOrder { get; set; }

		[JsonProperty("refs")]
		public int Refs { get; set; }

		[JsonProperty("is_avc")]
		public bool IsAvc { get; set; }

		[JsonProperty("nal_length_size")]
		public int NalLengthSize { get; set; }

		[JsonProperty("nb_frames")]
		public int NbFrames { get; set; }


		// Codec
		[JsonProperty("codec_name")]
		public string CodecName { get; set; }

		[JsonProperty("codec_name_long")]
		public string CodecNameLong { get; set; }

		[JsonProperty("codec_time_base")]
		public string CodecTimeBase { get; set; }

		[JsonProperty("codec_tag")]
		public string CodecTag { get; set; }

		[JsonProperty("codec_tag_string")]
		public string CodecTagString { get; set; }
	}
}
