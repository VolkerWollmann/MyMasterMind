using System;
using MyMasterMind.Interfaces;

namespace MyMasterMind.Model
{
	public class Evaluation : IMasterMindEvaluationModel
	{
		public int Black { get; internal set; }

		public int White { get; internal set; }

		#region IEqualityComparer<Evaluation>
		public bool Compare(Evaluation other)
		{
			return (this.White == other.White) && (this.Black == other.Black);
		}

		public int GetHashCode(Evaluation obj)
		{
			return Math.Abs(obj.Black | obj.White);
		}
		#endregion

		public Evaluation()
		{
			Black = 0;
			White = 0;
		}
	}
}
