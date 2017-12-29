using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stefmde.Tools.Console.ProgressHelper
{
	public class ConsoleProgressHelper
	{
		private readonly bool _showProgressBar;
		private readonly bool _showPercentage;
		private readonly bool _showElapsedTime;
		private readonly double _stepSize;
		private double _lastPrintedProgress;
		private readonly double _totalItemCount;
		private double _currentItemCount;
		private double _currentProgress;
		private string _currentLabel;
		private string _counterDivider;
		private readonly int _percentageRounding;
		private readonly int _progressBarLength;
		private readonly int _totalItemCountPadding;
		private readonly DateTime _initTime;
		public string Text { get; private set; }

		public ConsoleProgressHelper(double stepSize, int totalItemCount, bool showProgressBar = true, bool showPercentage = true, bool showElapsedTime = true, int progressBarLength = 20, int percentageRounding = 2, string counterDivider = "/")
		{

			if (stepSize <= 0)
			{
				throw new ArgumentException("must be greater than 0", "stepSize");
			}

			if (totalItemCount <= 0)
			{
				throw new ArgumentException("must be greater than 0", "totalItemCount");
			}

			if (progressBarLength <= 3)
			{
				throw new ArgumentException("must be greater than 3", "progressBarLength");
			}

			if (counterDivider.Length <= 0)
			{
				throw new ArgumentException("must be longer than 0", "counterDivider");
			}

			if (percentageRounding < 0 || percentageRounding > 10)
			{
				throw new ArgumentException("must between 0 and 10", "percentageRounding");
			}

			_showProgressBar = showProgressBar;
			_stepSize = stepSize;
			_totalItemCount = totalItemCount;
			_percentageRounding = percentageRounding;
			_progressBarLength = progressBarLength;
			_totalItemCountPadding = totalItemCount.ToString().Length;
			_counterDivider = counterDivider;
			_showPercentage = showPercentage;
			_showElapsedTime = showElapsedTime;
			_initTime = DateTime.Now;
		}

		/// <summary>
		/// Sets the current state of the application
		/// </summary>
		/// <param name="currentItemCount"></param>
		/// <param name="label"></param>
		/// <returns>Value if there is a new text to print in Text</returns>
		public bool Set(int currentItemCount, string label = "")
		{
			if (currentItemCount < 0)
			{
				throw new ArgumentException("must be greater than 0", "currentItemCount");
			}

			_currentItemCount = currentItemCount;
			_currentLabel = label;
			return ValidateProgress();
		}

		/// <summary>
		/// Sets the current state of the application
		/// </summary>
		/// <param name="currentItemCount"></param>
		/// <param name="label"></param>
		/// <returns>Value if there is a new text to print in Text</returns>
		public bool Set(double currentItemCount, string label = "")
		{
			if (currentItemCount < 0)
			{
				throw new ArgumentException("must be greater than 0", "currentItemCount");
			}

			_currentItemCount = currentItemCount;
			_currentLabel = label;
			return ValidateProgress();
		}

		private bool ValidateProgress()
		{
			_currentProgress = _currentItemCount / _totalItemCount * 100;

			double takenStep = _currentProgress - _lastPrintedProgress;

			if (takenStep >= _stepSize)
			{
				Print();
				return true;
			}

			return false;
		}

		private void Print()
		{
			StringBuilder shoutOutBuilder = new StringBuilder();

			if (_showProgressBar)
			{
				shoutOutBuilder.Append(GenerateProgressBar());
			}

			_lastPrintedProgress = _currentProgress;


			shoutOutBuilder.Append(_currentItemCount.ToString().PadLeft(_totalItemCountPadding, ' '));
			shoutOutBuilder.Append(_counterDivider);
			shoutOutBuilder.Append(_totalItemCount);


			if (_showPercentage)
			{
				int percentageSpacer = 0;
				if (_percentageRounding > 0)
				{
					percentageSpacer = 1;
				}

				shoutOutBuilder.Append("  ");
				shoutOutBuilder.Append(Math.Round(_currentProgress, _percentageRounding).ToString("F" + _percentageRounding).PadLeft(3 + _percentageRounding + percentageSpacer, ' '));
				shoutOutBuilder.Append(" %");
			}


			if (_showElapsedTime)
			{
				TimeSpan ts = DateTime.Now - _initTime;
				shoutOutBuilder.Append("  ");
				shoutOutBuilder.Append(ts.Hours.ToString().PadLeft(2, ' ') + "h");
				shoutOutBuilder.Append(" ");
				shoutOutBuilder.Append(ts.Minutes.ToString().PadLeft(2, ' ') + "m");
				shoutOutBuilder.Append(" ");
				shoutOutBuilder.Append(ts.Seconds.ToString().PadLeft(2, ' ') + "s");
			}


			shoutOutBuilder.Append("  ");
			shoutOutBuilder.Append(_currentLabel);

			Text = shoutOutBuilder.ToString();
		}

		private string GenerateProgressBar()
		{
			string bar = " [";

			double percentPerChar = (double)100 / _progressBarLength;
			int paintedChars = (int)Math.Round(_currentProgress / percentPerChar, 0);

			for (int i = 0; i < _progressBarLength; i++)
			{
				if (i < paintedChars)
				{
					bar += '#';
				}
				else
				{
					bar += ' ';
				}
			}

			bar += "]  ";

			return bar;
		}
	}
}
