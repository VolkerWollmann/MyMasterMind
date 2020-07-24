using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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
		Cyan
	}

	public enum MyMasterMindEvaluationColors
	{
		None=0,
		White,
		Black
	}

	public enum MyMasterMindCommands
	{
		Clear = 0,
		Computer,
		Cancel,
		User,
		Check,
	}

	public enum CellMark
	{
		None=0,
		ForInput,
		CompareFalse,
		CompareTrue,
	}
	public class MyMasterMindConstants
	{
		public const int CLOUMNS = 4;
		public const int ROWS = 10;

		public const string RED = "RED";
		public const string GREEN = "GREEN";
		public const string BLUE = "BLUE";
		public const string YELLOW = "YELLOW";
		public const string MAGENTA = "MAGENTA";
		public const string CYAN = "CYAN";

		private static readonly string[] COLORNAMES = { RED, GREEN, BLUE, YELLOW, MAGENTA, CYAN};

		public static MyMasterMindCodeColors GetCodeColor(string color)
		{
			List<Tuple<string, MyMasterMindCodeColors>> mapping =
				new List<Tuple<string, MyMasterMindCodeColors>>()
				{
					new Tuple<string, MyMasterMindCodeColors>( RED, MyMasterMindCodeColors.Red ),
					new Tuple<string, MyMasterMindCodeColors>( GREEN, MyMasterMindCodeColors.Green ),
					new Tuple<string, MyMasterMindCodeColors>( BLUE, MyMasterMindCodeColors.Blue ),
					new Tuple<string, MyMasterMindCodeColors>( YELLOW, MyMasterMindCodeColors.Yellow ),
					new Tuple<string, MyMasterMindCodeColors>( MAGENTA, MyMasterMindCodeColors.Magenta ),
					new Tuple<string, MyMasterMindCodeColors>( CYAN, MyMasterMindCodeColors.Cyan ),

				};

			if (mapping.Any(m => (m.Item1 == color)))
			{
				return mapping.First(m => (m.Item1 == color)).Item2;
			}

			return MyMasterMindCodeColors.None;
		}
	}
}
