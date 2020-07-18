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

		public static readonly string[] COLORNAMES = { RED, GREEN, BLUE, YELLOW, MAGENTA, CYAN};

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

	public class DisplayColors
	{
		private static DisplayColors singleton = null;
		private Brush[] CodeBrushes;
		private Brush[] EvaluationBrushes;

		public static Brush GetCodeBrush(MyMasterMindCodeColors color)
		{
			if (singleton == null)
				singleton = new DisplayColors();

			return singleton.CodeBrushes[(int)color];
	    }

		public static Brush GetEvaluationBrush(MyMasterMindEvaluationColors color)
		{
			if (singleton == null)
				singleton = new DisplayColors();

			return singleton.EvaluationBrushes[(int)color];
		}

		private DisplayColors()
		{
			CodeBrushes = new Brush[Enum.GetNames(typeof(MyMasterMindCodeColors)).Length];
			CodeBrushes[(int)MyMasterMindCodeColors.None]       = new SolidColorBrush(Colors.Gray);
			CodeBrushes[(int)MyMasterMindCodeColors.Red]		= new SolidColorBrush(Colors.Red);
			CodeBrushes[(int)MyMasterMindCodeColors.Green]		= new SolidColorBrush(Colors.Green);
			CodeBrushes[(int)MyMasterMindCodeColors.Blue]		= new SolidColorBrush(Colors.Blue);
			CodeBrushes[(int)MyMasterMindCodeColors.Yellow]		= new SolidColorBrush(Colors.Yellow);
			CodeBrushes[(int)MyMasterMindCodeColors.Magenta]	= new SolidColorBrush(Colors.Magenta);
			CodeBrushes[(int)MyMasterMindCodeColors.Cyan]		= new SolidColorBrush(Colors.Cyan);

			EvaluationBrushes = new Brush[Enum.GetNames(typeof(MyMasterMindEvaluationColors)).Length];
			EvaluationBrushes[(int)MyMasterMindEvaluationColors.None] = new SolidColorBrush(Colors.Gray);
			EvaluationBrushes[(int)MyMasterMindEvaluationColors.White] = new SolidColorBrush(Colors.White);
			EvaluationBrushes[(int)MyMasterMindEvaluationColors.Black] = new SolidColorBrush(Colors.Black);
		}
	}

}
