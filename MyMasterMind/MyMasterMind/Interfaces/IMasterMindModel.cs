using MyMasterMind.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMasterMind.Interfaces
{
	public interface IMasterMindCodeModel
	{
		MyMasterMindCodeColors[] Colors { get; }
	}

	public interface IMasterMindEvalutionModel
	{
		int Black { get; }
		int White { get; }
	}

	public interface IMasterMindGuessModel
	{
		IMasterMindCodeModel GetCode();

		IMasterMindEvalutionModel GetEvaluation();
	}

	public interface  IMasterMindGameModel
	{
		IMasterMindCodeModel GetCode();

		int GetCurrentGuessRow();

		IMasterMindGuessModel GetCurrentGuess();

		IMasterMindGuessModel GetNewGuess();

		bool Finished();

		IMasterMindGuessModel SetGuess(int row, MyMasterMindCodeColors[] code);

		//bool StartGetNewGuess();
		//Code GetNewUnEvaluatedGuess();
		//int GetFirstBadEvalaution();
	}
}
