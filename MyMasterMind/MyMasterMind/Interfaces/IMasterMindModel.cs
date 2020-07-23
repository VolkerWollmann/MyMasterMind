using MyMasterMind.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMasterMind.Interfaces
{
	interface  IMasterMindModel
	{
		Code Code { get; }

		int GetCurrentGuessRow();

		Guess GetCurrentGuess();

		Guess GetNewGuess();

		bool Finished();

		Guess SetGuess(int row, Code code);

		//bool StartGetNewGuess();
		//Code GetNewUnEvaluatedGuess();
		//int GetFirstBadEvalaution();
	}
}
