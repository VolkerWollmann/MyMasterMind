﻿using System;
using MyMasterMind.Interfaces;


namespace MyMasterMind.Model
{
	public class Guess
	{
		public Code Code { get; internal set; }
		internal Evaluation Evaluation { get;  set; }

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

		public Guess(Code code)
		{
			Code = code;
		}
	}
}
