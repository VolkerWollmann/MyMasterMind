using System;
using MyMasterMind.Interfaces;


namespace MyMasterMind.Model
{
	public class Guess
	{
		public Code Code { get; internal set; }
		public Evaluation Evaluation { get; internal set; }

		public static Guess GetRandomGuess()
		{
			Guess guess = new Guess();
			guess.Code = Code.GetRandomCode();

			return guess;
		}

		public Guess Copy()
		{
			Guess copy = new Guess();
			copy.Code = Code.Copy();

			return copy;
		}
		public void Increment()
		{
			Code.Increment();
		}

		internal bool Compare(Guess other)
		{
			return Code.Compare(other.Code).Equals(Evaluation);
		}

		public Guess()
		{
			Code = new Code();
		}
	}
}
