using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
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

		internal void Increment()
		{

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
					if ((i != j ) && (this[i] == copy[j]))
					{
						evaluation.White++;
						copy[j] = MyMasterMindCodeColors.None;
					}
				}
			}

			return evaluation;
		}

		

		internal static Code GetRandomCode()
		{
			Code code = new Code();

			Random random = new Random();
			Enumerable.Range(0,MyMasterMindConstants.CLOUMNS ).ToList().ForEach( i => 
				{ code[i] =(MyMasterMindCodeColors)random.Next(1, Enum.GetNames(typeof(MyMasterMindCodeColors)).Length); });

			return code;
		}
		public Code()
		{
			Colors = new MyMasterMindCodeColors[MyMasterMindConstants.CLOUMNS];
		}
	}
}
