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

			for(int i=0; i<4; i++)
			{
				Evaluation[i].Fill = DisplayColors.GetBrush(MyMasterMindColors.Gray);
				Field[i].Fill = DisplayColors.GetBrush(MyMasterMindColors.Gray);
			}
		}

		internal void SetColor( int column, MyMasterMindColors color )
		{
			Field[column].Fill = DisplayColors.GetBrush(color);
		}

		internal void SetEvaluation( int black, int white)
		{
			for (int i = 0; i < black; i++)
			{
				Evaluation[i].Fill = DisplayColors.GetBrush(MyMasterMindColors.Black);
			}
			for(int i= black; i<black+white; i++)
			{
				Evaluation[i].Fill = DisplayColors.GetBrush(MyMasterMindColors.White);
			}
			for (int i = black+white ; i < 4; i++)
			{
				Evaluation[i].Fill = DisplayColors.GetBrush(MyMasterMindColors.Gray);
			}
		}

		internal void HideEvaluation()
		{
			for (int i = 0; i < 4; i++)
				Evaluation[i].Visibility = Visibility.Hidden;
		}
	}
}
