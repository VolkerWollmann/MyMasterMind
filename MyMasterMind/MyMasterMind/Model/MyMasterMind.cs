using MyMasterMind.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMasterMind.Model
{
	using DataTriple = Tuple<int, int, int>;

	public class Code
	{
		public MyMasterMindCodeColors[] Colors { get; private set; }

		public static Code GetRandomCode()
		{
			Code code = new Code();

			Random random = new Random();
			for (int j = 0; j < MyMasterMindConstants.CLOUMNS; j++)
			{
				code.Colors[j] = (MyMasterMindCodeColors)random.Next(1, Enum.GetNames(typeof(MyMasterMindCodeColors)).Length);
			}

			return code;
		}
		public Code()
		{
			Colors = new MyMasterMindCodeColors[MyMasterMindConstants.CLOUMNS];
		}
	}

	public class Guess
	{
		public Code Code { get; private set; }
		public int Black { get; private set; }
		public int White { get; private set; }

		public static Guess GetRandomGuess()
		{
			Guess guess = new Guess();
			guess.Code = Code.GetRandomCode();

			Random random = new Random();
			guess.Black = random.Next(0, MyMasterMindConstants.CLOUMNS+1);
			guess.White = random.Next(0, 5 - guess.Black);

			return guess;
		}

		public Guess()
		{
			Code = new Code();
		}
	}
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
