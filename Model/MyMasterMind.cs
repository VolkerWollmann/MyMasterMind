using MyMasterMind.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMasterMind.Model
{
	using DataTriple = Tuple<int, int, int>;

	public class MyMasterMindGame : IMasterMindGameModel
	{
		private Code Code { get; }
		private readonly Guess[] Guesses;
		int CurrentGuessIndex;

		private Guess CurrentGuess
		{
			get => Guesses[CurrentGuessIndex];
            set => Guesses[CurrentGuessIndex] = value;
        }

		private Guess PreviousGuess => Guesses[CurrentGuessIndex-1];

        private List<Guess> GetGuessesSoFarAsList()
		{
			List<Guess> result = new List<Guess>();
			for(int i=0; i< CurrentGuessIndex; i++ )
			{
				result.Add(Guesses[i]);
			}

			return result;
		}
		public MyMasterMindGame()
		{
			Code = Code.GetRandomCode();
			Guesses = new Guess[MyMasterMindConstants.Rows];
			CurrentGuessIndex = -1;

		}

		public bool Finished()
		{
			return CurrentGuess.Evaluation.Black == MyMasterMindConstants.Columns;
		}

		public IMasterMindCodeModel GetCode()
		{
			return Code;
		}
		public IMasterMindGuessModel SetGuess(int row, MyMasterMindCodeColors[] code)
		{
			CurrentGuessIndex = row;
			CurrentGuess = new Guess(code);
			CurrentGuess.Evaluate(Code);
			return CurrentGuess;
		}

		public int GetCurrentGuessRow()
		{
			return CurrentGuessIndex;
		}
		public IMasterMindGuessModel GetCurrentGuess()
		{
			return CurrentGuess;
		}

		public Evaluation GetCurrentEvaluation()
		{
			return CurrentGuess.Evaluation;
		}

		#region Computer plays
		public IMasterMindGuessModel GetNewGuess()
		{
			CurrentGuessIndex++;
			if ( CurrentGuessIndex == 0)
				CurrentGuess = Guess.GetRandomGuess();
			else
			{
				CurrentGuess = PreviousGuess.Copy();
				List<Guess> guessesSoFar = GetGuessesSoFarAsList();
				do
				{
					CurrentGuess.Increment();
				}
				while (guessesSoFar.Any(guess => !guess.Compare(CurrentGuess)));

			}

			CurrentGuess.Evaluate(Code);
			return CurrentGuess;
		}

		public bool StartGetNewGuess()
		{
			CurrentGuessIndex++;
			CurrentGuess = CurrentGuessIndex == 0 ? Guess.GetRandomGuess() : PreviousGuess.Copy();

			return true;
		}

		public void Increment()
		{
			CurrentGuess.Increment();
		}

		public int GetFirstBadEvaluation()
		{
			for(int i=0; i < CurrentGuessIndex; i++ )
			{
				if (!Guesses[i].Compare(CurrentGuess))
					return i;
			}

			CurrentGuess.Evaluate(Code);

			return -1;
		}
		#endregion
	}
}
