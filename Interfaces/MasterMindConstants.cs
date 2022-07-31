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

	public class MyMasterMindConstants
	{
		public const int Columns = 4;
		public const int Rows = 10;

		public static MyMasterMindCodeColors MaxColor => Enum.GetValues(typeof(MyMasterMindCodeColors)).Cast<MyMasterMindCodeColors>().Max();
	}
}
