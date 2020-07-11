using System;
using System.Collections.Generic;

namespace MyMasterMind.Model
{
	public class Evaluation : IEqualityComparer<Evaluation>
	{
		public int Black { get; set; }

		public int White { get; set; }

		#region IEqualityComparer<Evaluation>
		public bool Equals(Evaluation x, Evaluation y)
		{
			return (x.White == y.White) && (x.Black == y.Black);
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
