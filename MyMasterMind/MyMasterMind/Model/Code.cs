using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using MyMasterMind.Interfaces;

namespace MyMasterMind.Model
{
	public class Code : IMasterMindCodeModel
	{
		static Random random = new Random();
		public MyMasterMindCodeColors[] Colors { get; private set; }

		internal MyMasterMindCodeColors this[int index]
		{
			get { return Colors[index]; }
			set { Colors[index] = value; }
		}
		internal Code Copy()
		{
			Code copy = new Code();

			copy.Colors = (MyMasterMindCodeColors[])Colors.Clone();
			
			return copy;
		}

		internal void Increment()
		{
			int index = 0;
			while (index < MyMasterMindConstants.CLOUMNS )
			{
				if (Colors[index] < MyMasterMindCodeColors.Cyan)
				{
					Colors[index] += 1;
					return;
				}
				else
				{
					Colors[index] = MyMasterMindCodeColors.Red;
					index++;
				}
			}
		}

		internal Evaluation Compare(Code other)
		{
			Evaluation evaluation = new Evaluation();

			Code otherCopy = other.Copy();
			Code myCopy = this.Copy();

			for(int i =0; i < MyMasterMindConstants.CLOUMNS; i++)
			{
				if ( myCopy[i] == otherCopy[i] ) 
				{
					evaluation.Black++;
					otherCopy[i] = MyMasterMindCodeColors.None;
					myCopy[i] = MyMasterMindCodeColors.None;
					
				}
			}

			for (int i = 0; i < MyMasterMindConstants.CLOUMNS; i++)
			{
				for (int j = 0; j < MyMasterMindConstants.CLOUMNS; j++)
				{
					if ((i != j ) && (myCopy[i] == otherCopy[j]) && ( otherCopy[j] != MyMasterMindCodeColors.None))
					{
						evaluation.White++;
						otherCopy[j] = MyMasterMindCodeColors.None;
						myCopy[i] = MyMasterMindCodeColors.None;
					}
				}
			}

			return evaluation;
		}

		

		internal static Code GetRandomCode()
		{
			Code code = new Code();

			Enumerable.Range(0,MyMasterMindConstants.CLOUMNS ).ToList().ForEach( i => 
				{ code[i] =(MyMasterMindCodeColors)random.Next(1, Enum.GetNames(typeof(MyMasterMindCodeColors)).Length); });

			return code;
		}
		public Code()
		{
			Colors = new MyMasterMindCodeColors[MyMasterMindConstants.CLOUMNS];
		}

		public Code(MyMasterMindCodeColors[] code)
		{
			Colors = new MyMasterMindCodeColors[MyMasterMindConstants.CLOUMNS];
			for(int i=0; i< MyMasterMindConstants.CLOUMNS; i++)
			{
				Colors[i] = code[i];
			}
		}
	}
}
