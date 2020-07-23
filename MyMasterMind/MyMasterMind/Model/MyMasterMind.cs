using MyMasterMind.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMasterMind.Model
{
	using DataTriple = Tuple<int, int, int>;

	public class MyMasterMindGame : IMasterMindGameModel
	{
		private Code Code { get; set; }
		private Guess[] Guesses;
		int currentGuessIndex;

		private Guess CurrentGuess
		{
			get { return Guesses[currentGuessIndex]; }
			set { Guesses[currentGuessIndex] = value; }
		}

		private Guess PreviousGuess
		{
			get { return Guesses[currentGuessIndex-1]; }
		}

		private List<Guess> GetGuessesSoFarAsList()
		{
			List<Guess> result = new List<Guess>();
			for(int i=0; i< currentGuessIndex; i++ )
			{
				result.Add(Guesses[i]);
			}

			return result;
		}
		public MyMasterMindGame()
		{
			Code = Code.GetRandomCode();
			Guesses = new Guess[MyMasterMindConstants.ROWS];
			currentGuessIndex = -1;

		}

		public bool Finished()
		{
			return CurrentGuess.Evaluation.Black == MyMasterMindConstants.CLOUMNS;
		}

		public IMasterMindCodeModel GetCode()
		{
			return Code;
		}
		public IMasterMindGuessModel SetGuess(int row, MyMasterMindCodeColors[] code)
		{
			currentGuessIndex = row;
			CurrentGuess = new Guess(code);
			CurrentGuess.Evaluate(Code);
			return CurrentGuess;
		}

		public int GetCurrentGuessRow()
		{
			return currentGuessIndex;
		}
		public IMasterMindGuessModel GetCurrentGuess()
		{
			return CurrentGuess;
		}

		public Evaluation GetCurrentEvalaution()
		{
			return CurrentGuess.Evaluation;
		}

		public IMasterMindGuessModel GetNewGuess()
		{
			currentGuessIndex++;
			if ( currentGuessIndex == 0)
				CurrentGuess = Guess.GetRandomGuess();
			else
			{
				CurrentGuess = PreviousGuess.Copy();
				List<Guess> guesseSoFar = GetGuessesSoFarAsList();
				do
				{
					CurrentGuess.Increment();
				}
				while (guesseSoFar.Any(guess => !guess.Compare(CurrentGuess)));

			}
			CurrentGuess.Evaluate(Code);
			return CurrentGuess;
		}

	}
}
