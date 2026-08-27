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

	/// <summary>
	/// How a single gene (column) of an individual of the genetic search came to be:
	/// from the initial random population, carried over unchanged as an elite, taken
	/// from the first or second parent by crossover, or created by mutation.
	/// </summary>
	public enum GeneticGeneOrigin
	{
		Random = 0,
		Carried,
		FirstParent,
		SecondParent,
		Mutation,
	}

	/// <summary>
	/// State of the genetic search for the guess of the current row.
	/// </summary>
	public interface IGeneticGenerationInfo
	{
		int Generation { get; }

		/// <summary>
		/// Fitness of the best individual: total deviation of its comparison against
		/// the guesses so far from their recorded evaluations. 0 means consistent.
		/// </summary>
		int BestFitness { get; }

		bool Consistent { get; }

		/// <summary>
		/// Origin of each gene of the best individual, so the recombination that
		/// created it can be visualized.
		/// </summary>
		GeneticGeneOrigin[] BestOrigins { get; }

		/// <summary>
		/// Crossover point of the best individual: genes before it come from the
		/// first parent, genes from it on from the second. 0 if the best individual
		/// was not bred (random start individual or carried-over elite).
		/// </summary>
		int BestCrossoverPoint { get; }
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

		#region Computer plays with genetic strategy
		/// <summary>
		/// Start the genetic search for the guess of the next row: advances to the
		/// next row and creates a fresh random population.
		/// </summary>
		bool StartGeneticGuess();

		/// <summary>
		/// Evolve the population by one generation and make the best individual the
		/// current (unevaluated) guess. Returns true when the search is finished:
		/// either the best individual is consistent with all guesses so far, or the
		/// generation limit is reached — the genetic strategy is allowed to fail.
		/// </summary>
		bool GeneticStep();

		/// <summary>
		/// State of the genetic search started with StartGeneticGuess.
		/// </summary>
		IGeneticGenerationInfo GetGeneticGenerationInfo();

		/// <summary>
		/// Play the best individual found: evaluate the current guess against the code.
		/// </summary>
		IMasterMindGuessModel CommitGeneticGuess();
		#endregion
	}
}
