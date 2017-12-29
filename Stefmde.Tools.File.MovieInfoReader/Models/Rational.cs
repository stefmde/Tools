namespace Stefmde.Tools.File.MovieInfoReader.Models
{
	public class Rational
	{
		public int Numerator { get; set; }
		public int Deumerator { get; set; }

		public override string ToString()
		{
			return Numerator + "/" + Deumerator;
		}
	}
}
