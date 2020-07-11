using System;
using MyMasterMind.Interfaces;

namespace MyMasterMind.Model
{
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
}
