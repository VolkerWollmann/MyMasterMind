using MyMasterMind.Interfaces;
using System;
using System.Windows.Media;

namespace MyMasterMind.Controls
{
	public class DisplayColors
	{
		private static DisplayColors _singleton;
		private readonly Brush[] CodeBrushes;
		private readonly Brush[] EvaluationBrushes;

		public static Brush GetCodeBrush(MyMasterMindCodeColors color)
		{
			if (_singleton == null)
				_singleton = new DisplayColors();

			return _singleton.CodeBrushes[(int)color];
		}

		public static Brush GetEvaluationBrush(MyMasterMindEvaluationColors color)
		{
			if (_singleton == null)
				_singleton = new DisplayColors();

			return _singleton.EvaluationBrushes[(int)color];
		}

		private DisplayColors()
		{
			CodeBrushes = new Brush[Enum.GetNames(typeof(MyMasterMindCodeColors)).Length];
			CodeBrushes[(int)MyMasterMindCodeColors.None] = new SolidColorBrush(Colors.Gray);
			CodeBrushes[(int)MyMasterMindCodeColors.Red] = new SolidColorBrush(Colors.Red);
			CodeBrushes[(int)MyMasterMindCodeColors.Green] = new SolidColorBrush(Colors.Green);
			CodeBrushes[(int)MyMasterMindCodeColors.Blue] = new SolidColorBrush(Colors.Blue);
			CodeBrushes[(int)MyMasterMindCodeColors.Yellow] = new SolidColorBrush(Colors.Yellow);
			CodeBrushes[(int)MyMasterMindCodeColors.Magenta] = new SolidColorBrush(Colors.Magenta);
			CodeBrushes[(int)MyMasterMindCodeColors.Cyan] = new SolidColorBrush(Colors.Cyan);
			CodeBrushes[(int)MyMasterMindCodeColors.SandyBrown] = new SolidColorBrush(Colors.SandyBrown);

			EvaluationBrushes = new Brush[Enum.GetNames(typeof(MyMasterMindEvaluationColors)).Length];
			EvaluationBrushes[(int)MyMasterMindEvaluationColors.None] = new SolidColorBrush(Colors.Gray);
			EvaluationBrushes[(int)MyMasterMindEvaluationColors.White] = new SolidColorBrush(Colors.White);
			EvaluationBrushes[(int)MyMasterMindEvaluationColors.Black] = new SolidColorBrush(Colors.Black);
		}
	}
}
