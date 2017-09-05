using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stefmde.File.Hash.Models
{
	public class HashResult
	{
		public FileInfo FileInfo { get; set; }
		public string HashBeginning { get; set; }
		public string HashEnding { get; set; }
		public bool Success { get; set; }
		public int HashedBytes { get; set; }
		public TimeSpan Duration { get; set; }



		public string HashCombined
		{
			get { return HashBeginning + ":" + HashEnding; }
		}

		public override string ToString()
		{
			return "File: " + FileInfo.Name + "  Hash: " + HashCombined;
		}
	}
}
