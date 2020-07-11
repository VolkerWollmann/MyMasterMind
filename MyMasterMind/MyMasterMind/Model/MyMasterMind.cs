using MyMasterMind.Interfaces;
using System;
using System.Collections.Generic;


namespace MyMasterMind.Model
{
	using DataTriple = Tuple<int, int, int>;

	public class MyMasterMindGame
	{
		public Code Code { get; private set; }
		private Guess[] Guesses;
		int currentGuess;

		public MyMasterMindGame()
		{
			Code = Code.GetRandomCode();
			Guesses = new Guess[MyMasterMindConstants.ROWS];
			currentGuess = -1;

		}
		public Guess GetGuess()
		{
			currentGuess++;
			Guesses[currentGuess] = Guess.GetRandomGuess();
			return Guesses[currentGuess];
		}
	}
}
