using System;
using MyMasterMind.Interfaces;


namespace MyMasterMind.Model
{
	public class Guess
	{
		public Code Code { get; private set; }
		public Evaluation Evaluation { get; internal set; }

		public static Guess GetRandomGuess()
		{
			Guess guess = new Guess();
			guess.Code = Code.GetRandomCode();

			return guess;
		}

		public Guess()
		{
			Code = new Code();
		}
	}
}
