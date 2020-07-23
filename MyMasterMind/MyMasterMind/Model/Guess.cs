using System;
using MyMasterMind.Interfaces;


namespace MyMasterMind.Model
{
	public class Guess : IMasterMindGuessModel
	{
		internal Code Code { get; set; }
		internal Evaluation Evaluation { get;  set; }

		public static Guess GetRandomGuess()
		{
			Guess guess = new Guess();
			guess.Code = Code.GetRandomCode();

			return guess;
		}

		public IMasterMindCodeModel GetCode()
		{
			return Code;
		}

		public IMasterMindEvalutionModel GetEvaluation()
		{
			return Evaluation;
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
			return Code.Compare(other.Code).Compare(Evaluation);
		}

		internal void Evaluate(Code code)
		{
			Evaluation = Code.Compare(code);
		}

		public Guess()
		{
			Code = new Code();
		}

		public Guess(MyMasterMindCodeColors[] code)
		{
			Code = new Code(code);
		}
	}
}
