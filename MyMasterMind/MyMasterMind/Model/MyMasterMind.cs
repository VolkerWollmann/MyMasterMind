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
		int currentGuess;

		private Guess CurrentGuess
		{
			get { return Guesses[currentGuess]; }
			set { Guesses[currentGuess] = value; }
		}

		private Guess PreviousGuess
		{
			get { return Guesses[currentGuess-1]; }
		}

		private List<Guess> GetGuessesSoFarAsList()
		{
			List<Guess> result = new List<Guess>();
			for(int i=0; i< currentGuess; i++ )
			{
				result.Add(Guesses[i]);
			}

			return result;
		}
		public MyMasterMindGame()
		{
			Code = Code.GetRandomCode();
			Guesses = new Guess[MyMasterMindConstants.ROWS];
			currentGuess = -1;

		}
		public Guess GetGuess()
		{
			currentGuess++;
			if ( currentGuess == 1)
				CurrentGuess = Guess.GetRandomGuess();
			else
			{
				CurrentGuess = PreviousGuess.Copy();
				CurrentGuess.Increment();
				while (GetGuessesSoFarAsList().Any(guess => !guess.Compare(CurrentGuess)))
				{
					CurrentGuess.Increment();
				}
			}
			CurrentGuess.Evaluation = Code.Compare(CurrentGuess.Code);
			return CurrentGuess;
		}
	}
}
