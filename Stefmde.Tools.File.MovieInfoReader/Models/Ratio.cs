namespace Stefmde.Tools.File.MovieInfoReader.Models
{
	public class Ratio
	{
		public Ratio(int leftSide, int rightSide)
		{
			LeftSide = leftSide;
			RightSide = rightSide;
		}

		public int LeftSide { get; set; }
		public int RightSide { get; set; }

		public override string ToString()
		{
			return LeftSide + ":" + RightSide;
		}
	}
}
