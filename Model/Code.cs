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

			foreach (MyMasterMindComparisonDetail detail in CompareDetails(other))
			{
				if (detail.Contribution == MyMasterMindEvaluationColors.Black)
					evaluation.Black++;
				else if (detail.Contribution == MyMasterMindEvaluationColors.White)
					evaluation.White++;
			}

			return evaluation;
		}

		/// <summary>
		/// For each column of this code: whether it counts as a black or a white
		/// evaluation against the other code (including the matched column of the
		/// other code), or does not contribute (None).
		/// </summary>
		public MyMasterMindComparisonDetail[] CompareDetails(Code other)
		{
			MyMasterMindComparisonDetail[] details = new MyMasterMindComparisonDetail[MyMasterMindConstants.Columns];

			Code otherCopy = other.Copy();
			Code myCopy = this.Copy();

			for(int i =0; i < MyMasterMindConstants.Columns; i++)
			{
				details[i] = new MyMasterMindComparisonDetail(MyMasterMindEvaluationColors.None, -1);
				if ( (myCopy[i] == otherCopy[i]) && (myCopy[i] != MyMasterMindCodeColors.None) )
				{
					details[i] = new MyMasterMindComparisonDetail(MyMasterMindEvaluationColors.Black, i);
					otherCopy[i] = MyMasterMindCodeColors.None;
					myCopy[i] = MyMasterMindCodeColors.None;

				}
			}

			for (int i = 0; i < MyMasterMindConstants.Columns; i++)
			{
				for (int j = 0; j < MyMasterMindConstants.Columns; j++)
				{
					if ((myCopy[i] == otherCopy[j]) && ( otherCopy[j] != MyMasterMindCodeColors.None))
					{
						details[i] = new MyMasterMindComparisonDetail(MyMasterMindEvaluationColors.White, j);
						otherCopy[j] = MyMasterMindCodeColors.None;
						myCopy[i] = MyMasterMindCodeColors.None;
					}
				}
			}

			return details;
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
