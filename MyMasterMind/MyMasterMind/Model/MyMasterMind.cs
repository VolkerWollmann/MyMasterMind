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
			Random random = new Random();
			for (int j = 0; j < MyMasterMindConstants.CLOUMNS; j++)
			{
				guess.Code.Colors[j] = (MyMasterMindCodeColors)random.Next(1, Enum.GetNames(typeof(MyMasterMindCodeColors)).Length);
			}
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
		public MyMasterMindGame()
		{
		}
		public Guess GetGuess()
		{
			return Guess.GetRandomGuess();
		}
	}
}
