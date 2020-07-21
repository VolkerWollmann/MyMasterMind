using MyMasterMind.Controls;
using MyMasterMind.Interfaces;
using MyMasterMind.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace MyMasterMind.ViewModel
{
	using DataTriple = Tuple<int, int, int>;
	public class MyMasterMindViewModel
	{
		MasterMindBoard MasterMindBoard;
		IMasterMindCommandView MasterMindCommands;
		MyMasterMindGame Game;
		int CurrentGuessRow;

		private void ClearBoard()
		{
			for (int j = 0; j < MyMasterMindConstants.CLOUMNS; j++)
			{
				MasterMindBoard.SetCodeColor(j, MyMasterMindCodeColors.None);
			}

			for (int i=0; i< MyMasterMindConstants.ROWS; i++)
			{
				for (int j = 0; j < MyMasterMindConstants.CLOUMNS; j++)
					MasterMindBoard.SetGuessColor(i, j, MyMasterMindCodeColors.None);

				MasterMindBoard.SetGuessEvaluation(i, 0, 0);
				MasterMindBoard.MarkGuessCell(i, false);
			}
		}

		private void ShowCode()
		{
			for (int j = 0; j < MyMasterMindConstants.CLOUMNS; j++)
			{
				MasterMindBoard.SetCodeColor(j, Game.Code.Colors[j]);
			}

		}

		#region Constructor
		public MyMasterMindViewModel(MasterMindBoard masterMindBoard, MasterMindCommands masterMindCommands)
		{
			MasterMindBoard = masterMindBoard;
			MasterMindCommands = (IMasterMindCommandView)masterMindCommands;

			// bind commands to buttons
			MasterMindCommands.SetClearCommandEventHandler(ClearCommand);
			MasterMindCommands.SetComputerCommandEventHandler(ComputerCommand);
			MasterMindCommands.SetUserCommandEventHandler(UserCommand);
			MasterMindCommands.SetCheckCommandEventHandler(CheckCommand);
		}

		#endregion

		#region Commands
		private void ClearCommand(object sender, EventArgs e)
		{
			ClearBoard();
			MasterMindCommands.DisableButton(MyMasterMindCommands.Check);
			MasterMindCommands.EnableButton(MyMasterMindCommands.Computer);
			MasterMindCommands.EnableButton(MyMasterMindCommands.User);
		}

		BackgroundWorker BackgroundWorker;
		Guess Guess;

		private void BackgroundWorkerComputerProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (Guess != null)
			{
				for (int j = 0; j < MyMasterMindConstants.CLOUMNS; j++)
				{
					MasterMindBoard.SetGuessColor(CurrentGuessRow, j, Guess.Code.Colors[j]);
				}
				MasterMindBoard.SetGuessEvaluation(CurrentGuessRow, Guess.Evaluation.Black, Guess.Evaluation.White);
			}
		}

		void BackGroundComputerDoWork(object sender, DoWorkEventArgs e)
		{
			Guess = null;
			CurrentGuessRow = 0;

			ShowCode();

			BackgroundWorker.ReportProgress((CurrentGuessRow + 1) * 10);

			for (int i = 0; i < MyMasterMindConstants.ROWS; i++)
			{
				Guess = Game.GetGuess();
				CurrentGuessRow = i;
				BackgroundWorker.ReportProgress((CurrentGuessRow + 1) * 10);
				System.Threading.Thread.Sleep(100);
				if (Game.Finished())
					break;
			}
		}

		void BackGroundComputerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			MasterMindCommands.EnableButton(MyMasterMindCommands.Computer);
			MasterMindCommands.EnableButton(MyMasterMindCommands.User);
		}


		private void ComputerCommand(object sender, EventArgs e)
		{
			ClearBoard();
			MasterMindCommands.DisableButton(MyMasterMindCommands.Check);
			MasterMindCommands.DisableButton(MyMasterMindCommands.User);
			MasterMindCommands.DisableButton(MyMasterMindCommands.Computer);
			
			Game = new MyMasterMindGame();
			BackgroundWorker = new BackgroundWorker();
			BackgroundWorker.WorkerReportsProgress = true;
			BackgroundWorker.DoWork += BackGroundComputerDoWork;
			BackgroundWorker.RunWorkerCompleted += BackGroundComputerCompleted;
			BackgroundWorker.ProgressChanged += BackgroundWorkerComputerProgressChanged;
			BackgroundWorker.RunWorkerAsync(this);
		}

		private void UserCommand(object sender, EventArgs e)
		{
			ClearBoard();
			
			MasterMindCommands.DisableButton(MyMasterMindCommands.User);
			MasterMindCommands.DisableButton(MyMasterMindCommands.Computer);
			MasterMindCommands.EnableButton(MyMasterMindCommands.Check);

			Game = new MyMasterMindGame();
			
			CurrentGuessRow = 0;
			MasterMindBoard.MarkGuessCell(CurrentGuessRow, true);
		}

		private void CheckCommand(object sender, EventArgs e)
		{
			Code code = new Code();
			for(int i = 0; i < MyMasterMindConstants.CLOUMNS; i++ )
			{
				code[i] = MasterMindBoard.GetGuessColor(CurrentGuessRow, i);
			}

			Guess = Game.SetGuess(CurrentGuessRow, code);
			MasterMindBoard.SetGuessEvaluation(CurrentGuessRow, Guess.Evaluation.Black, Guess.Evaluation.White);

			CurrentGuessRow++;
			if ( (CurrentGuessRow >= MyMasterMindConstants.ROWS) || Game.Finished() )
			{
				ShowCode();
				MasterMindCommands.EnableButton(MyMasterMindCommands.User);
				MasterMindCommands.EnableButton(MyMasterMindCommands.Computer);
				MasterMindCommands.DisableButton(MyMasterMindCommands.Check);
				return;
			}
			MasterMindBoard.MarkGuessCell(CurrentGuessRow, true); ;
		}
		#endregion
	}
}
