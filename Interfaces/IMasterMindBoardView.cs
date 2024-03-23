namespace MyMasterMind.Interfaces
{
	public interface IMasterMindBoardView
	{
		void SetCode(MyMasterMindCodeColors[] colors);

		void SetGuessColors(int row, MyMasterMindCodeColors[] color);

		MyMasterMindCodeColors GetGuessColor(int row, int column);

		void SetGuessEvaluation(int row, int black, int white);

		void MarkGuessCell(int row, CellMark mark);

        void Clear();
    }
}
