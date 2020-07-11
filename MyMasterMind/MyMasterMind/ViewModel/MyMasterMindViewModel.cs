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

		private void ClearBoard()
		{
			for(int j=0; j<MyMasterMindConstants.CLOUMNS; j++)
			{
				MasterMindBoard.SetCodeColor(j, MyMasterMindCodeColors.None);
			}

			for(int i=0; i< MyMasterMindConstants.ROWS; i++)
			{
				for (int j = 0; j < MyMasterMindConstants.CLOUMNS; j++)
					MasterMindBoard.SetGuessColor(i, j, MyMasterMindCodeColors.None);

				MasterMindBoard.SetGuessEvaluation(i, 0, 0);
			}
		}

		#region Construcotr
		public MyMasterMindViewModel(MasterMindBoard masterMindBoard, MasterMindCommands masterMindCommands)
		{
			MasterMindBoard = masterMindBoard;
			MasterMindCommands = (IMasterMindCommandView)masterMindCommands;

			// bind command to buttons
			MasterMindCommands.SetClearCommandEventHandler(ClearCommand);
			MasterMindCommands.SetComputerCommandEventHandler(ComputerCommand);
		}

		#endregion

		#region Commands
		private void ClearCommand(object sender, EventArgs e)
		{
			ClearBoard();
		}

		BackgroundWorker BackgroundWorker;
		Guess Guess;
		int CurrentGuess;
		bool ShowCode;

		private void BackgroundWorkerComputerProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (ShowCode)
			{
				for (int j = 0; j < MyMasterMindConstants.CLOUMNS; j++)
				{
					MasterMindBoard.SetCodeColor(j, Game.Code.Colors[j]);
				}

				ShowCode = false;
			}

			if (Guess != null)
			{
				for (int j = 0; j < MyMasterMindConstants.CLOUMNS; j++)
				{
					MasterMindBoard.SetGuessColor(CurrentGuess, j, Guess.Code.Colors[j]);
				}
				MasterMindBoard.SetGuessEvaluation(CurrentGuess, Guess.Evaluation.Black, Guess.Evaluation.White);
			}
		}

		void BackGroundComputerDoWork(object sender, DoWorkEventArgs e)
		{
			Guess = null;
			CurrentGuess = 0;
			
			ShowCode = true;
			BackgroundWorker.ReportProgress((CurrentGuess + 1) * 10);

			for (int i = 0; i < MyMasterMindConstants.ROWS; i++)
			{
				Guess = Game.GetGuess();
				CurrentGuess = i;
				BackgroundWorker.ReportProgress((CurrentGuess + 1) * 10);
				System.Threading.Thread.Sleep(100);
			}
		}

		void BackGroundComputerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			//
		}


		private void ComputerCommand(object sender, EventArgs e)
		{
			Game = new MyMasterMindGame();
			BackgroundWorker = new BackgroundWorker();
			BackgroundWorker.WorkerReportsProgress = true;
			BackgroundWorker.DoWork += BackGroundComputerDoWork;
			BackgroundWorker.RunWorkerCompleted += BackGroundComputerCompleted;
			BackgroundWorker.ProgressChanged += BackgroundWorkerComputerProgressChanged;
			BackgroundWorker.RunWorkerAsync(this);
		}

		#endregion
	}
}
