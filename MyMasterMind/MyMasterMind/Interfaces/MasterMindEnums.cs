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
	}
}
