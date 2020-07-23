using System;
using System.Collections.Generic;

namespace MyMasterMind.Model
{
	internal class Evaluation 
	{
		public int Black { get; set; }

		public int White { get; set; }

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
