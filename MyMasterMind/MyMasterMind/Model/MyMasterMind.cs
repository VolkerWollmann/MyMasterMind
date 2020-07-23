using MyMasterMind.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyMasterMind.Model
{
	using DataTriple = Tuple<int, int, int>;

	public class MyMasterMindGame
	{
		public Code Code { get; private set; }
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

		public Guess SetGuess(int row, Code code)
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
		public Guess GetCurrentGuess()
		{
			return CurrentGuess;
		}

		public Guess GetNewGuess()
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
