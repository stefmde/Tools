using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stefmde.Tools.File.ConfigurationManager;
using Stefmde.Tools.File.MovieInfoReader;
using Stefmde.Tools.File.MovieInfoReader.Models;

namespace Stefmde.Tools.Test.Console
{
	class Program
	{
		static void Main(string[] args)
		{

			MovieInfoReader reader = new MovieInfoReader(@"C:\Users\Stefm\Desktop\ffmpeg-3.4.1-win64-static\bin\ffprobe.exe");

			MovieInfo info = reader.Read(
				@"M:\Filmreihen\Der.König.der.Löwen\Der.Koenig.der.Loewen.1.1994.German.DL.DTS.1080p.BluRay.x264.m4v");










			//TestConfiguration configuration = new TestConfiguration()
			//{
			//	Test = "BlahBlah"
			//};


			//JsonConfigurationManager<TestConfiguration> configurationManager = new JsonConfigurationManager<TestConfiguration>();
			////configurationManager.WriteConfiguration(@"D:\", configuration);

			//TestConfiguration configuration2 = configurationManager.ReadConfiguration(@"D:\");

			System.Console.ReadKey();
		}
	}
}
