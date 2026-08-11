namespace MyMasterMind.Interfaces
{
	public interface IMasterMindBoardView
	{
		void SetCode(MyMasterMindCodeColors[] colors);

		void SetGuessColors(int row, MyMasterMindCodeColors[] color);

		MyMasterMindCodeColors[] GetGuessColors(int row);

		void SetGuessEvaluation(int row, int black, int white);

		void MarkGuessCell(int row, CellMark mark);

		/// <summary>
		/// Mark a single field of a guess row with its contribution to a comparison:
		/// black or white evaluation, or no contribution (None).
		/// </summary>
		void MarkGuessField(int row, int column, MyMasterMindEvaluationColors contribution);

		/// <summary>
		/// Remove the contribution mark from a single field of a guess row.
		/// </summary>
		void UnmarkGuessField(int row, int column);

        void Clear();
    }
}
