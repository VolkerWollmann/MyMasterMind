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
		public MyMasterMindColors[] Colors { get; private set; }
		public int Black { get; private set; }
		public int White { get; private set; }

		public Guess()
		{
			Random random = new Random();
			Colors = new MyMasterMindColors[4];
			for (int j = 0; j < 4; j++)
			{
				Colors[j] = (MyMasterMindColors)random.Next(3, 9);
			}
			Black = random.Next(0, 5);
			White = random.Next(0, 5 - Black);

		}
	}
	public class MyMasterMindGame
	{
		public MyMasterMindGame()
		{
		}
		public Guess GetGuess()
		{
			return new Guess();
		}
	}
}
