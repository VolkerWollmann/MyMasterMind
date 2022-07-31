using System;
using System.Linq;
using MyMasterMind.Interfaces;

namespace MyMasterMind.Model
{
	public class Code : IMasterMindCodeModel
	{
		static readonly Random Random = new Random();
		public MyMasterMindCodeColors[] Colors { get; private set; }

		internal MyMasterMindCodeColors this[int index]
		{
			get => Colors[index];
            set => Colors[index] = value;
        }
		internal Code Copy()
		{
            Code copy = new Code {Colors = (MyMasterMindCodeColors[]) Colors.Clone()};


            return copy;
		}

		internal void Increment()
		{
			int index = 0;
			while (index < MyMasterMindConstants.Columns )
			{
				if (Colors[index] < MyMasterMindConstants.MaxColor)
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

		public Evaluation Compare(Code other)
		{
			Evaluation evaluation = new Evaluation();

			Code otherCopy = other.Copy();
			Code myCopy = this.Copy();

			for(int i =0; i < MyMasterMindConstants.Columns; i++)
			{
				if ( myCopy[i] == otherCopy[i] ) 
				{
					evaluation.Black++;
					otherCopy[i] = MyMasterMindCodeColors.None;
					myCopy[i] = MyMasterMindCodeColors.None;
					
				}
			}

			for (int i = 0; i < MyMasterMindConstants.Columns; i++)
			{
				for (int j = 0; j < MyMasterMindConstants.Columns; j++)
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

			Enumerable.Range(0,MyMasterMindConstants.Columns ).ToList().ForEach( i => 
				{ code[i] =(MyMasterMindCodeColors)Random.Next(1, Enum.GetNames(typeof(MyMasterMindCodeColors)).Length); });

			return code;
		}
		public Code()
		{
			Colors = new MyMasterMindCodeColors[MyMasterMindConstants.Columns];
		}

		public Code(MyMasterMindCodeColors[] code)
		{
			Colors = new MyMasterMindCodeColors[MyMasterMindConstants.Columns];
			for(int i=0; i< MyMasterMindConstants.Columns; i++)
			{
				Colors[i] = code[i];
			}
		}
	}
}
