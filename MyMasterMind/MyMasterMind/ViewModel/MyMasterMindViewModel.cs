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
	using ComputerPlayInformation = Tuple<int, CellMark>;
	public class MyMasterMindViewModel
	{
		IMasterMindBoardView MasterMindBoard;
		IMasterMindCommandView MasterMindCommands;
		IMasterMindGameModel Game;

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
				MasterMindBoard.MarkGuessCell(i, CellMark.None);
			}
		}

		private void ShowCode()
		{
			for (int j = 0; j < MyMasterMindConstants.CLOUMNS; j++)
			{
				MasterMindBoard.SetCodeColor(j, Game.GetCode().Colors[j]);
			}

		}

		private void EnableCommands(List<MyMasterMindCommands> commandList)
		{
			commandList.ForEach(command => { MasterMindCommands.EnableButton(command); });

		}

		private void DisableCommands(List<MyMasterMindCommands> commandList)
		{
			commandList.ForEach(command => { MasterMindCommands.DisableButton(command); });

		}

		#region Constructor
		public MyMasterMindViewModel(MasterMindBoard masterMindBoard, MasterMindCommands masterMindCommands)
		{
			MasterMindBoard = masterMindBoard;
			MasterMindCommands = (IMasterMindCommandView)masterMindCommands;

			// bind commands to buttons
			MasterMindCommands.SetCommandEventHandler(MyMasterMindCommands.Clear,        ClearCommand);
			MasterMindCommands.SetCommandEventHandler(MyMasterMindCommands.ComputerSlow, ComputerSlowCommand);
			MasterMindCommands.SetCommandEventHandler(MyMasterMindCommands.ComputerFast, ComputerFastCommand);
			MasterMindCommands.SetCommandEventHandler(MyMasterMindCommands.Cancel,       CancelCommand);
			MasterMindCommands.SetCommandEventHandler(MyMasterMindCommands.User,         UserCommand);
			MasterMindCommands.SetCommandEventHandler(MyMasterMindCommands.Check,        CheckCommand);
		}

		#endregion

		#region Commands
		private void ClearCommand(object sender, EventArgs e)
		{
			ClearBoard();
			DisableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.Check, MyMasterMindCommands.Cancel});
			EnableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.ComputerSlow, MyMasterMindCommands.ComputerFast, MyMasterMindCommands.User, MyMasterMindCommands.Clear });
		}

		#region Computer Command
		MyMasterMindCommands computerCommand;
		BackgroundWorker BackgroundWorker;

		private void BackgroundWorkerComputerProgressChanged(object sender, ProgressChangedEventArgs e)
		{

			IMasterMindGuessModel guess = Game.GetCurrentGuess();
			if (guess != null)
			{
				int currentGuessRow = Game.GetCurrentGuessRow();
				for (int j = 0; j < MyMasterMindConstants.CLOUMNS; j++)
				{
					MasterMindBoard.SetGuessColor(currentGuessRow, j, guess.GetCode().Colors[j]);
				}
				if (guess.GetEvaluation() != null )
					MasterMindBoard.SetGuessEvaluation(currentGuessRow, guess.GetEvaluation().Black, guess.GetEvaluation().White);
			}

			if (e.UserState != null)
			{
				ComputerPlayInformation computerPlayInformation = (ComputerPlayInformation)e.UserState;
				MasterMindBoard.MarkGuessCell(computerPlayInformation.Item1, computerPlayInformation.Item2);
			}
		}

		private void BackGroundComputerDoWork(object sender, DoWorkEventArgs e)
		{
			ShowCode();

			for (int i = 0; i < MyMasterMindConstants.ROWS; i++)
			{
				if (computerCommand == MyMasterMindCommands.ComputerSlow)
				{
					int firstBadEvaluation;

					Game.StartGetNewGuess();

					int currentGuessRow = Game.GetCurrentGuessRow();

					BackgroundWorker.ReportProgress(0, null);

					do
					{
						Game.Increment();
						firstBadEvaluation = Game.GetFirstBadEvalaution();

						// show evaluations
						int jMax = (firstBadEvaluation > -1) ? Math.Min(firstBadEvaluation, Game.GetCurrentGuessRow()) : Game.GetCurrentGuessRow();
						int badblinkrate = 25;

						for (int j = 0; j < jMax; j++)
						{
							// show the good evalaution
							BackgroundWorker.ReportProgress(0, new ComputerPlayInformation(j, CellMark.CompareTrue));
							System.Threading.Thread.Sleep(500);
							BackgroundWorker.ReportProgress(0, new ComputerPlayInformation(j, CellMark.None));
							System.Threading.Thread.Sleep(500);

							badblinkrate = 500;
						}

						if (firstBadEvaluation > -1)
						{
							// show the first bad evalaution
							BackgroundWorker.ReportProgress(0, new ComputerPlayInformation(firstBadEvaluation, CellMark.CompareFalse));
							System.Threading.Thread.Sleep(badblinkrate);
							BackgroundWorker.ReportProgress(0, new ComputerPlayInformation(firstBadEvaluation, CellMark.None));
							System.Threading.Thread.Sleep(badblinkrate);
						}

						if (BackgroundWorker.CancellationPending)
							return;

					} while (firstBadEvaluation > -1);
				}
				else
				{
					Game.GetNewGuess();

					BackgroundWorker.ReportProgress(0, null);

					System.Threading.Thread.Sleep(500);
				}

				BackgroundWorker.ReportProgress(0, null);
				System.Threading.Thread.Sleep(100);
				if (Game.Finished())
					break;
			}
		}

		private void BackGroundComputerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			DisableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.Check, MyMasterMindCommands.Cancel });
			EnableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.ComputerSlow, MyMasterMindCommands.ComputerFast, MyMasterMindCommands.User, MyMasterMindCommands.Clear });
		}


		private void ComputerComand(MyMasterMindCommands command)
		{
			ClearBoard();
			EnableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.Cancel });
			DisableCommands(new List<MyMasterMindCommands>() { 
				MyMasterMindCommands.ComputerSlow, MyMasterMindCommands.ComputerFast, MyMasterMindCommands.User, MyMasterMindCommands.Clear, MyMasterMindCommands.Check });

			Game = new MyMasterMindGame();
			BackgroundWorker = new BackgroundWorker();
			BackgroundWorker.WorkerReportsProgress = true;
			BackgroundWorker.DoWork += BackGroundComputerDoWork;
			BackgroundWorker.RunWorkerCompleted += BackGroundComputerCompleted;
			BackgroundWorker.ProgressChanged += BackgroundWorkerComputerProgressChanged;
			BackgroundWorker.WorkerSupportsCancellation = true;
			BackgroundWorker.RunWorkerAsync(this);
		}

		private void ComputerSlowCommand(object sender, EventArgs e)
		{
			computerCommand = MyMasterMindCommands.ComputerSlow;
			ComputerComand(MyMasterMindCommands.ComputerSlow);
		}

		private void ComputerFastCommand(object sender, EventArgs e)
		{
			computerCommand = MyMasterMindCommands.ComputerFast;
			ComputerComand(MyMasterMindCommands.ComputerFast);
		}
		#endregion

		private void CancelCommand(object sender, EventArgs e)
		{
			if (BackgroundWorker != null)
				BackgroundWorker.CancelAsync();
		}

		private void UserCommand(object sender, EventArgs e)
		{
			ClearBoard();

			EnableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.Check, MyMasterMindCommands.Clear });
			DisableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.ComputerSlow, MyMasterMindCommands.ComputerFast, MyMasterMindCommands.User, MyMasterMindCommands.Cancel });

			MasterMindCommands.

			Game = new MyMasterMindGame();

			MasterMindBoard.MarkGuessCell(0, CellMark.ForInput);
		}

		private void CheckCommand(object sender, EventArgs e)
		{
			MyMasterMindCodeColors[] code = new MyMasterMindCodeColors[MyMasterMindConstants.ROWS];
			int currentGuessRow = Game.GetCurrentGuessRow()+1;
			for (int i = 0; i < MyMasterMindConstants.CLOUMNS; i++ )
			{
				code[i] = MasterMindBoard.GetGuessColor(currentGuessRow, i);
			}

			IMasterMindGuessModel guess = Game.SetGuess(currentGuessRow, code);
			MasterMindBoard.SetGuessEvaluation(currentGuessRow, guess.GetEvaluation().Black, guess.GetEvaluation().White);

			MasterMindBoard.MarkGuessCell(currentGuessRow, CellMark.None);
			currentGuessRow++;
			if ( (currentGuessRow >= MyMasterMindConstants.ROWS) || Game.Finished() )
			{
				ShowCode();
				DisableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.Check, MyMasterMindCommands.Cancel });
				EnableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.ComputerSlow, MyMasterMindCommands.ComputerFast, MyMasterMindCommands.User, MyMasterMindCommands.Clear });
				return;
			}
			MasterMindBoard.MarkGuessCell(currentGuessRow, CellMark.ForInput ); 
		}
		#endregion
	}
}
