using MyMasterMind.Interfaces;
using System;
using System.Linq;
using System.Windows.Controls;

namespace MyMasterMind.Controls
{
	/// <summary>
	/// Interaction logic for MasterMindBoard.xaml
	/// </summary>
	public partial class MasterMindBoard : IMasterMindBoardView, ISetCheckCheckCommandEventHandler
	{
		private readonly GuessCell Code;
		private readonly GuessCell[] GuessCells = new GuessCell[MyMasterMindConstants.Rows];

		public MasterMindBoard()
		{
			InitializeComponent();

			Code = new GuessCell();
			BoardGrid.Children.Add(Code);
			Grid.SetColumn(Code, 0);
			Grid.SetRow(Code, 0);

			for (int i = 0; i < MyMasterMindConstants.Rows; i++)
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


		public void MarkGuessCell(int row, CellMark mark )
		{
			GuessCells[row].Mark(mark);
		}

		public MyMasterMindCodeColors GetGuessColor(int row, int column)
		{
			return GuessCells[row].GetColor(column);
		}

		#region ISetCheckCheckCommandEventHandler
		public void SetCheckCheckCommandEventHandler(EventHandler checkCheckCommandEventHandler)
        {
			GuessCells.ToList().ForEach(gc => { gc.SetCheckCheckCommandEventHandler(checkCheckCommandEventHandler); });
		}
		#endregion
	}
}
