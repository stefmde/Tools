using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Stefmde.Tools.File.MovieInfoReader.Helper;
using Stefmde.Tools.File.MovieInfoReader.Models;

namespace Stefmde.Tools.File.MovieInfoReader
{
	public class MovieInfoReader
	{
		private FileInfo _movieFile;
		private readonly object _outputLockObject = new object();
		private readonly object _errorLockObject = new object();
		private readonly string _exePath;

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

			if (String.IsNullOrEmpty(json))
			{
				return null;
			}
			else
			{
				JsonParser parser = new JsonParser();
				return parser.ReadJson(json);
			}
		}

		private string InspectFile()
		{
			string json = "";

			int timeout = 10000;
			//string exePath = "Externals\\ffprobe.exe";
			string parms = "-show_streams -show_chapters -print_format json -sexagesimal \"" + _movieFile + "\"";

			// Process source: https://stackoverflow.com/a/7608823/7245313
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
					}
				}
			}

			return json;
		}
	}
}