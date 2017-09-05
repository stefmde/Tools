using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stefmde.File.Hash.Models
{
	public enum Accuracy
	{

		/// <summary>
		/// Hash first and last 10 KB
		/// </summary> 
		UltraLow = 10240,

		/// <summary>
		/// Hash first and last 100 KB
		/// </summary> 
		Low = 102400,


		/// <summary>
		/// Hash first and last 1 MB
		/// </summary>
		Medium = 1024000,


		/// <summary>
		/// Hash first and last 10 MB
		/// </summary>
		High = 10240000,

		/// <summary>
		/// Hash first and last 100 MB
		/// </summary>
		UltraHigh = 102400000
	}
}
