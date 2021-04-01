using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using MyMasterMind.Interfaces;

namespace MyMasterMind.Controls
{
	/// <summary>
	/// Interaction logic for GuessCell.xaml
	/// </summary>
	public partial class GuessCell : ISetCheckCheckCommandEventHandler
	{
		private readonly Rectangle[] Evaluation;
		private readonly CodeField[] Field;
		public GuessCell()
		{
			InitializeComponent();

			this.DataContext = this;

			Evaluation = new Rectangle[4];
			Field = new CodeField[4];

			Evaluation[0] = Evaluation0;
			Evaluation[1] = Evaluation1;
			Evaluation[2] = Evaluation2;
			Evaluation[3] = Evaluation3;

			for (int i = 0; i < MyMasterMindConstants.Columns; i++)
			{
				Field[i] = new CodeField();
				this.CodeStackPanel.Children.Add(Field[i]);
			}

			Evaluation.ToList().ForEach(e => { e.Fill = DisplayColors.GetEvaluationBrush(MyMasterMindEvaluationColors.None); });
			Field.ToList().ForEach(e => { e.SetColor( MyMasterMindCodeColors.None); });
		}

		internal void SetColor( int column, MyMasterMindCodeColors color )
		{
			Field[column].SetColor(color);
		}

		internal MyMasterMindCodeColors GetColor( int column )
		{
			return Field[column].GetColor();
		}

		internal void SetEvaluation( int black, int white)
		{
			int i;
			Evaluation.ToList().ForEach(e => { e.Fill = DisplayColors.GetEvaluationBrush(MyMasterMindEvaluationColors.None); }); 
			for (i = 0; i < black; i++)
			{
				Evaluation[i].Fill = DisplayColors.GetEvaluationBrush(MyMasterMindEvaluationColors.Black);
			}
			for(i= black; i<black+white; i++)
			{
				Evaluation[i].Fill = DisplayColors.GetEvaluationBrush(MyMasterMindEvaluationColors.White);
			}
		}

		internal void HideEvaluation()
		{
			Evaluation.ToList().ForEach(r => { r.Visibility = Visibility.Hidden; });
		}

		internal void Mark(CellMark mark)
		{
			Field.ToList().ForEach(f => { f.DisableMenu(); });

			switch(mark)
			{
				case CellMark.None:
					EvaluationStackPanel.Background = new SolidColorBrush(Colors.White);
					CodeStackPanel.Background = new SolidColorBrush(Colors.White);
					break;

				case CellMark.ForInput:
					GradientStop gs1 = new GradientStop(Colors.White, 0);
					GradientStop gs2 = new GradientStop(Colors.White, 0.5);
					GradientStop gs3 = new GradientStop(Colors.Gray, 0.5);
					GradientStop gs4 = new GradientStop(Colors.Gray, 1);
					LinearGradientBrush lgb = new LinearGradientBrush();
					lgb.MappingMode = BrushMappingMode.Absolute;
					lgb.StartPoint = new Point(0, 0);
					lgb.EndPoint = new Point(4, 4);
					lgb.SpreadMethod = GradientSpreadMethod.Repeat;
					lgb.GradientStops.Add(gs1);
					lgb.GradientStops.Add(gs2);
					lgb.GradientStops.Add(gs3);
					lgb.GradientStops.Add(gs4);
					EvaluationStackPanel.Background = lgb;
					CodeStackPanel.Background = lgb;
					Field.ToList().ForEach(f => { f.EnableMenu(); });
					break;

				case CellMark.CompareTrue:
					EvaluationStackPanel.Background = new SolidColorBrush(Colors.LightGreen);
					CodeStackPanel.Background = new SolidColorBrush(Colors.LightGreen);
					break;

				case CellMark.CompareFalse:
					EvaluationStackPanel.Background = new SolidColorBrush(Colors.IndianRed);
					CodeStackPanel.Background = new SolidColorBrush(Colors.IndianRed);
					break;
			}
		}

		#region ISetCheckCheckCommandEventHandler
		public void SetCheckCheckCommandEventHandler(EventHandler checkCheckCommandEventHandler)
        {
			Field.ToList().ForEach(f => {f.SetCheckCheckCommandEventHandler(checkCheckCommandEventHandler); } );
		}
		#endregion
	}
}
