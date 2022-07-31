namespace MyMasterMind.Interfaces
{
	public interface IMasterMindBoardView
	{
		void SetCodeColor(int column, MyMasterMindCodeColors color);

		void SetGuessColor(int row, int column, MyMasterMindCodeColors color);

		MyMasterMindCodeColors GetGuessColor(int row, int column);

		void SetGuessEvaluation(int row, int black, int white);

		void MarkGuessCell(int row, CellMark mark);
	}
}
