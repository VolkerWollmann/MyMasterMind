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

		public void SetCode(MyMasterMindCodeColors[] colors)
		{
            for (int j = 0; j < MyMasterMindConstants.Columns; j++ )
                 Code.SetColor(j, colors[j]);
		}
		public void SetGuessColors(int row, MyMasterMindCodeColors[] colors)
		{
			for( int j = 0;j < MyMasterMindConstants.Columns; j++)
			    GuessCells[row].SetColor(j, colors[j]);
		}

		public void SetGuessEvaluation(int row, int black, int white)
		{
			GuessCells[row].SetEvaluation(black, white);
		}


		public void MarkGuessCell(int row, CellMark mark )
		{
			GuessCells[row].Mark(mark);
		}

		public MyMasterMindCodeColors[] GetGuessColors(int row)
        {
            MyMasterMindCodeColors[] guessColors = new MyMasterMindCodeColors[MyMasterMindConstants.Columns];
            for (int j = 0; j < MyMasterMindConstants.Columns; j++)
                guessColors[j] = GuessCells[row].GetColor(j);

            return guessColors;

        }

        public void Clear()
        {
            MyMasterMindCodeColors[] emptyColorsArray = new MyMasterMindCodeColors[MyMasterMindConstants.Columns];
			
            for (int j = 0; j < MyMasterMindConstants.Columns; j++)
                emptyColorsArray[j] = MyMasterMindCodeColors.None;
            
            SetCode(emptyColorsArray);

            for (int i = 0; i < MyMasterMindConstants.Rows; i++)
            {
                SetGuessColors(i, emptyColorsArray);
                SetGuessEvaluation(i, 0, 0);
                MarkGuessCell(i, CellMark.None);
            }
        }
		
		#region ISetCheckCheckCommandEventHandler
		public void SetCheckCheckCommandEventHandler(EventHandler checkCheckCommandEventHandler)
        {
			GuessCells.ToList().ForEach(gc => { gc.SetCheckCheckCommandEventHandler(checkCheckCommandEventHandler); });
		}
		#endregion
	}
}
