using System;
using System.Runtime.CompilerServices;
using MyMasterMind.Interfaces;

namespace MyMasterMind.Model
{
	public class Code
	{
		public MyMasterMindCodeColors[] Colors { get; private set; }

		internal MyMasterMindCodeColors this[int index]
		{
			get { return Colors[index]; }
			set { Colors[index] = value; }
		}
		internal Code Copy()
		{
			Code copy = new Code();
			
			for(int i = 0; i< MyMasterMindConstants.CLOUMNS; i++ )
			{
				copy[i] = this[i];
			}

			return copy;
		}
		internal Evaluation Compare(Code other)
		{
			Evaluation evaluation = new Evaluation();

			Code copy = other.Copy();

			for(int i =0; i < MyMasterMindConstants.CLOUMNS; i++)
			{
				if ( this[i] == copy[i] )
				{
					evaluation.Black++;
					copy[i] = MyMasterMindCodeColors.None;
				}
			}

			for (int i = 0; i < MyMasterMindConstants.CLOUMNS; i++)
			{
				for (int j = 0; j < MyMasterMindConstants.CLOUMNS; j++)
				{
					if ((i != j ) && (this[i] == copy[i]))
					{
						evaluation.White++;
						copy[i] = MyMasterMindCodeColors.None;
					}
				}
			}

			return evaluation;
		}
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
