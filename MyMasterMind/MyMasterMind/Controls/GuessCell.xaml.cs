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
		private Rectangle[] Field;
		public GuessCell()
		{
			InitializeComponent();

			this.DataContext = this;

			Evaluation = new Rectangle[4];
			Field = new Rectangle[4];

			Evaluation[0] = Evaluation0;
			Evaluation[1] = Evaluation1;
			Evaluation[2] = Evaluation2;
			Evaluation[3] = Evaluation3;

			Field[0] = Field0;
			Field[1] = Field1;
			Field[2] = Field2;
			Field[3] = Field3;

			Evaluation.Cast<Rectangle>().ToList().ForEach(e => { e.Fill = DisplayColors.GetCodeBrush(MyMasterMindCodeColors.None); });
			Field.Cast<Rectangle>().ToList().ForEach(e => { e.Fill = DisplayColors.GetEvaluationBrush(MyMasterMindEvaluationColors.None); });
		}

		public ICommand SelectColorCommand
		{
			get
			{
				return new SelectColorCommand();
			}
		}

		internal void SetColor( int column, MyMasterMindCodeColors color )
		{
			Field[column].Fill = DisplayColors.GetCodeBrush(color);
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
	}
}
