namespace MyMasterMind.Interfaces
{
	public interface IMasterMindBoardView
	{
		void SetCode(MyMasterMindCodeColors[] colors);

		void SetGuessColor(int row, int column, MyMasterMindCodeColors color);

		MyMasterMindCodeColors GetGuessColor(int row, int column);

		void SetGuessEvaluation(int row, int black, int white);

		void MarkGuessCell(int row, CellMark mark);

        void Clear();
    }
}
