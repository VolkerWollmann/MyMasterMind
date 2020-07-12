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

		public bool Finished()
		{
			return CurrentGuess.Evaluation.Black == MyMasterMindConstants.CLOUMNS;
		}
		public Guess GetGuess()
		{
			int j = 0;
			currentGuess++;
			if ( currentGuess == 0)
				CurrentGuess = Guess.GetRandomGuess();
			else
			{
				CurrentGuess = new Guess();
				CurrentGuess.Code[0] = MyMasterMindCodeColors.Magenta;
				CurrentGuess.Code[1] = MyMasterMindCodeColors.Magenta;
				CurrentGuess.Code[2] = MyMasterMindCodeColors.Magenta;
				CurrentGuess.Code[3] = MyMasterMindCodeColors.Cyan;
				//CurrentGuess = PreviousGuess.Copy();
				List<Guess> guesseSoFar = GetGuessesSoFarAsList();
				while (guesseSoFar.Any(guess => !guess.Compare(CurrentGuess)))
				{
					j++;
					if ( j > ( 6*6*6*6 ))
					{
						;
					}
					CurrentGuess.Increment();
				}
			}
			CurrentGuess.Evaluation = Code.Compare(CurrentGuess.Code);
			return CurrentGuess;
		}
	}
}
