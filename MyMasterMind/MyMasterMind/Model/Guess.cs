using System;
using MyMasterMind.Interfaces;


namespace MyMasterMind.Model
{
	public class Guess
	{
		public Code Code { get; private set; }
		public Evaluation Evaluation { get; private set; }

		public static Guess GetRandomGuess()
		{
			Guess guess = new Guess();
			guess.Code = Code.GetRandomCode();

			Random random = new Random();
			guess.Evaluation = new Evaluation();
			guess.Evaluation.Black = random.Next(0, MyMasterMindConstants.CLOUMNS + 1);
			guess.Evaluation.White = random.Next(0, 5 - guess.Evaluation.Black);

			return guess;
		}

		public Guess()
		{
			Code = new Code();
		}
	}
}
