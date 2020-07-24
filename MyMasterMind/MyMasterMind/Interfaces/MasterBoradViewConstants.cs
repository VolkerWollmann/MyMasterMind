using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMasterMind.Interfaces
{
	public enum CellMark
	{
		None = 0,
		ForInput,
		CompareFalse,
		CompareTrue,
	}

	public enum MyMasterMindCommands
	{
		Clear = 0,
		Computer,
		Cancel,
		User,
		Check,
	}
}
