using System;
using System.Linq;

namespace MyMasterMind.Interfaces
{
	
	public enum MyMasterMindCodeColors
	{
		None=0,
		Red,
		Green,
		Blue,
		Yellow,
		Magenta,
		Cyan,
		SandyBrown
	}

	public enum MyMasterMindEvaluationColors
	{
		None=0,
		White,
		Black
	}

	/// <summary>
	/// How the computer searches for its next guess: exhaustive enumeration
	/// (guaranteed to find a consistent guess) or a genetic algorithm, which
	/// may fail to solve the game within the available rows.
	/// </summary>
	public enum MyMasterMindStrategy
	{
		Enumeration = 0,
		Genetic,
	}

	public class MyMasterMindConstants
	{
		public const int Columns = 4;
		public const int Rows = 10;

		public static MyMasterMindCodeColors MaxColor => Enum.GetValues(typeof(MyMasterMindCodeColors)).Cast<MyMasterMindCodeColors>().Max();
	}
}
