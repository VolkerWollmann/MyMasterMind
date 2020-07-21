using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using MyMasterMind.Interfaces;
using MyMasterMind.Commands;

namespace MyMasterMind.Controls
{
	/// <summary>
	/// Interaction logic for GuessCell.xaml
	/// </summary>
	public partial class GuessCell : UserControl
	{
		private Rectangle[] Evaluation;
		private CodeField[] Field;
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

			for (int i = 0; i < MyMasterMindConstants.CLOUMNS; i++)
			{
				Field[i] = new CodeField();
				this.CodeStackPanel.Children.Add(Field[i]);
			}

			Evaluation.Cast<Rectangle>().ToList().ForEach(e => { e.Fill = DisplayColors.GetEvaluationBrush(MyMasterMindEvaluationColors.None); });
			Field.Cast<CodeField >().ToList().ForEach(e => { e.SetColor( MyMasterMindCodeColors.None); });
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
			Evaluation.Cast<Rectangle>().ToList().ForEach(e => { e.Fill = DisplayColors.GetEvaluationBrush(MyMasterMindEvaluationColors.None); }); 
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
			Evaluation.Cast<Rectangle>().ToList().ForEach(r => { r.Visibility = Visibility.Hidden; });
		}

		internal void Mark( bool mark)
		{
			if (mark)
			{
				EvaluationStackPanel.Background = new SolidColorBrush(Colors.Aqua);
				CodeStackPanel.Background = new SolidColorBrush(Colors.Aqua);
				Field.Cast<CodeField>().ToList().ForEach(f => { f.EnableMenu(); });
			}
			else
			{
				EvaluationStackPanel.Background = new SolidColorBrush(Colors.White);
				CodeStackPanel.Background = new SolidColorBrush(Colors.White);
				Field.Cast<CodeField>().ToList().ForEach(f => { f.DisableMenu(); });
			}
		}
	}
}
