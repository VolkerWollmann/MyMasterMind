using MyMasterMind.Controls;
using MyMasterMind.Interfaces;
using MyMasterMind.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
		MyMasterMindGame Game = new MyMasterMindGame();

		private void ClearBoard()
		{
			for(int i=0; i< MyMasterMindConstants.ROWS; i++)
			{
				for (int j = 0; j < MyMasterMindConstants.CLOUMNS; j++)
					MasterMindBoard.SetColor(i, j, MyMasterMindCodeColors.None);

				MasterMindBoard.SetEvaluation(i, 0, 0);
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

		BackgroundWorker backgroundWorker;
		Guess guess;
		int currentGuess;

		private void BackgroundWorkerComputerProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (guess != null)
			{
				for (int j = 0; j < MyMasterMindConstants.CLOUMNS; j++)
				{
					MasterMindBoard.SetColor(currentGuess, j, guess.Code.Colors[j]);
				}
				MasterMindBoard.SetEvaluation(currentGuess, guess.Black, guess.White);
			}
		}

		void BackGroundComputerDoWork(object sender, DoWorkEventArgs e)
		{
			guess = null;
			currentGuess = 0;
			for (int i = 0; i < MyMasterMindConstants.ROWS; i++)
			{
				guess = Game.GetGuess();
				currentGuess = i;
				backgroundWorker.ReportProgress((currentGuess + 1) * 10);
				System.Threading.Thread.Sleep(100);
			}
		}

		void BackGroundComputerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			//
		}


		private void ComputerCommand(object sender, EventArgs e)
		{
			backgroundWorker = new BackgroundWorker();
			backgroundWorker.WorkerReportsProgress = true;
			backgroundWorker.DoWork += BackGroundComputerDoWork;
			backgroundWorker.RunWorkerCompleted += BackGroundComputerCompleted;
			backgroundWorker.ProgressChanged += BackgroundWorkerComputerProgressChanged;
			backgroundWorker.RunWorkerAsync(this);
		}

		#endregion
	}
}
