namespace MyMasterMind.Interfaces
{
	public interface IMasterMindCodeModel
	{
		MyMasterMindCodeColors[] Colors { get; }
	}

	public interface IMasterMindEvaluationModel
	{
		int Black { get; }
		int White { get; }
	}

	/// <summary>
	/// Contribution of a single field of a guess to a comparison with another code:
	/// black or white evaluation (or none) and the matched column in the other code.
	/// </summary>
	public class MyMasterMindComparisonDetail
	{
		public MyMasterMindEvaluationColors Contribution { get; }

		/// <summary>
		/// Column of the other code this field is matched with, -1 if it does not contribute.
		/// </summary>
		public int OtherColumn { get; }

		public MyMasterMindComparisonDetail(MyMasterMindEvaluationColors contribution, int otherColumn)
		{
			Contribution = contribution;
			OtherColumn = otherColumn;
		}
	}

	public interface IMasterMindGuessModel
	{
		IMasterMindCodeModel GetCode();

		IMasterMindEvaluationModel GetEvaluation();
	}

	public interface  IMasterMindGameModel
	{
		IMasterMindCodeModel GetCode();

		int GetCurrentGuessRow();

		IMasterMindGuessModel GetCurrentGuess();

		
		bool Finished();

		#region User plays
		IMasterMindGuessModel SetGuess(int row, MyMasterMindCodeColors[] code);
		#endregion

		#region Computer plays
		/// <summary>
		/// Get a new guess, which is consistent with guesses so far, at once
		/// </summary>
		/// <returns></returns>
		IMasterMindGuessModel GetNewGuess();

		/// <summary>
		/// Start generation of a new consistent guess.
		/// </summary>
		/// <returns></returns>
		bool StartGetNewGuess();

		/// <summary>
		/// Prepare next unevaluated code in current guess.
		/// </summary>
		/// <returns></returns>
		void Increment();

		/// <summary>
		/// Get row index of guesses so far, with which the unevaluated code does not match.
		/// If this is -1,  GetCurrentGuess will return the new consistent guess
		/// and the process, which was started with StartGetNewGuess is finished.
		/// </summary>
		/// <returns></returns>
		int GetFirstBadEvaluation();

		/// <summary>
		/// For each field of the current (unevaluated) guess: whether it would count as
		/// a black or a white evaluation against the guess in the given row (including
		/// the matched column of that row), or does not contribute (None).
		/// </summary>
		/// <returns></returns>
		MyMasterMindComparisonDetail[] GetComparisonDetails(int row);
		#endregion

	}
}
