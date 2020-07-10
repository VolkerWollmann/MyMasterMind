using MyMasterMind.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMasterMind.Model
{
	using DataTriple = Tuple<int, int, int>;

	public class Guess
	{
		public MyMasterMindCodeColors[] Colors { get; private set; }
		public int Black { get; private set; }
		public int White { get; private set; }

		public static Guess GetRandomGuess()
		{
			Guess guess = new Guess();
			Random random = new Random();
			guess.Colors = new MyMasterMindCodeColors[4];
			for (int j = 0; j < 4; j++)
			{
				guess.Colors[j] = (MyMasterMindCodeColors)random.Next(1, 7);
			}
			guess.Black = random.Next(0, 5);
			guess.White = random.Next(0, 5 - guess.Black);

			return guess;
		}

		public Guess()
		{

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
