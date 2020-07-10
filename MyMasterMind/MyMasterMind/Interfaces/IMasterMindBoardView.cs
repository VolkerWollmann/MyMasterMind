using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMasterMind.Interfaces
{
	interface IMasterMindBoardView
	{
		void SetColor(int row, int column, MyMasterMindCodeColors color);

		void SetEvaluation(int row, int black, int white);
	}
}
