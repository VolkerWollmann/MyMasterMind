using MyMasterMind.Interfaces;
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

namespace MyMasterMind.Controls
{
	/// <summary>
	/// Interaction logic for MasterMindBoard.xaml
	/// </summary>
	public partial class MasterMindBoard : UserControl, IMasterMindBoardView
	{
		private GuessCell Code;
		private GuessCell[] GuessCells = new GuessCell[MyMasterMindConstants.ROWS];
		private GuessCell CurrentGuessCell = null;

		public MasterMindBoard()
		{
			InitializeComponent();

			Code = new GuessCell();
			BoardGrid.Children.Add(Code);
			Grid.SetColumn(Code, 0);
			Grid.SetRow(Code, 0);

			for (int i = 0; i < MyMasterMindConstants.ROWS; i++)
			{
				GuessCells[i] = new GuessCell();
				BoardGrid.Children.Add(GuessCells[i]);
				Grid.SetColumn(GuessCells[i], 0);
				Grid.SetRow(GuessCells[i], 2 + (9-i));
			}

			Code.HideEvaluation();
		}

		public void SetCodeColor(int column, MyMasterMindCodeColors color)
		{
			Code.SetColor(column, color);
		}
		public void SetGuessColor(int row, int column, MyMasterMindCodeColors color)
		{
			GuessCells[row].SetColor(column, color);
		}

		public void SetGuessEvaluation(int row, int black, int white)
		{
			GuessCells[row].SetEvaluation(black, white);
		}


		public void MarkGuessCell(int row, bool mark )
		{
			if (CurrentGuessCell != null)
				CurrentGuessCell.Mark(false);

			CurrentGuessCell = GuessCells[row];
			CurrentGuessCell.Mark(mark);
		}

		public MyMasterMindCodeColors GetGuessColor(int row, int column)
		{
			return GuessCells[row].GetColor(column);
		}
	}
}
