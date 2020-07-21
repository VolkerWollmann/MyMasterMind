using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMasterMind.Interfaces
{
	interface IMasterMindBoardView
	{
		void SetCodeColor(int column, MyMasterMindCodeColors color);

		void SetGuessColor(int row, int column, MyMasterMindCodeColors color);

		void SetGuessEvaluation(int row, int black, int white);

		void MarkGuessCell(int row, bool mark);
	}
}
