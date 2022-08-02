using MyMasterMind.Interfaces;
using MyMasterMind.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MyMasterMind.ViewModel
{
	using ComputerPlayInformation = Tuple<int, CellMark>;
	public class MyMasterMindViewModel
	{
        readonly IMasterMindBoardView MasterMindBoard;
        readonly IMasterMindCommandView MasterMindCommands;
		IMasterMindGameModel Game;
		bool UserPlaying;

		private void ClearBoard()
		{
			for (int j = 0; j < MyMasterMindConstants.Columns; j++)
			{
				MasterMindBoard.SetCodeColor(j, MyMasterMindCodeColors.None);
			}

			for (int i=0; i< MyMasterMindConstants.Rows; i++)
			{
				for (int j = 0; j < MyMasterMindConstants.Columns; j++)
					MasterMindBoard.SetGuessColor(i, j, MyMasterMindCodeColors.None);

				MasterMindBoard.SetGuessEvaluation(i, 0, 0);
				MasterMindBoard.MarkGuessCell(i, CellMark.None);
			}
		}

		private void ShowCode()
		{
			for (int j = 0; j < MyMasterMindConstants.Columns; j++)
			{
				MasterMindBoard.SetCodeColor(j, Game.GetCode().Colors[j]);
			}

		}

		private void EnableCommands(List<MyMasterMindCommands> commandList)
		{
			commandList.ForEach(command => { MasterMindCommands.SetButtonState(command, true); });

		}

		private void DisableCommands(List<MyMasterMindCommands> commandList)
		{
			commandList.ForEach(command => { MasterMindCommands.SetButtonState(command, false); });

		}

		#region Constructor
		public MyMasterMindViewModel(IMasterMindBoardView masterMindBoard, IMasterMindCommandView masterMindCommands)
		{
			MasterMindBoard = masterMindBoard;

			((ISetCheckCheckCommandEventHandler)MasterMindBoard).SetCheckCheckCommandEventHandler(CheckCheckCommand);

			MasterMindCommands = masterMindCommands;

			// bind commands to buttons
			MasterMindCommands.SetCommandEventHandler(MyMasterMindCommands.Clear,        ClearCommand);
			MasterMindCommands.SetCommandEventHandler(MyMasterMindCommands.ComputerSlow, ComputerSlowCommand);
			MasterMindCommands.SetCommandEventHandler(MyMasterMindCommands.ComputerFast, ComputerFastCommand);
			MasterMindCommands.SetCommandEventHandler(MyMasterMindCommands.Cancel,       CancelCommand);
			MasterMindCommands.SetCommandEventHandler(MyMasterMindCommands.User,         UserCommand);
			MasterMindCommands.SetCommandEventHandler(MyMasterMindCommands.Check,        CheckCommand);

			DisableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.Check, MyMasterMindCommands.Cancel });
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
		MyMasterMindCommands ComputerCommand;
		BackgroundWorker BackgroundWorker;

		private void BackgroundWorkerComputerProgressChanged(object sender, ProgressChangedEventArgs e)
		{

			IMasterMindGuessModel guess = Game.GetCurrentGuess();
			if (guess != null)
			{
				int currentGuessRow = Game.GetCurrentGuessRow();
				for (int j = 0; j < MyMasterMindConstants.Columns; j++)
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

			for (int i = 0; i < MyMasterMindConstants.Rows; i++)
			{
				if (ComputerCommand == MyMasterMindCommands.ComputerSlow)
				{
					int firstBadEvaluation;

					Game.StartGetNewGuess();

					Game.GetCurrentGuessRow();

					BackgroundWorker.ReportProgress(0, null);

					do
					{
						Game.Increment();
						firstBadEvaluation = Game.GetFirstBadEvaluation();

						// show evaluations
						int jMax = (firstBadEvaluation > -1) ? Math.Min(firstBadEvaluation, Game.GetCurrentGuessRow()) : Game.GetCurrentGuessRow();

						for (int j = 0; j < jMax; j++)
						{
							// show the good evaluation
							BackgroundWorker.ReportProgress(0, new ComputerPlayInformation(j, CellMark.CompareTrue));
							System.Threading.Thread.Sleep(MyMasterMindBoarViewConstants.GoodGuessDisplayTime);
							BackgroundWorker.ReportProgress(0, new ComputerPlayInformation(j, CellMark.None));
							System.Threading.Thread.Sleep(MyMasterMindBoarViewConstants.GoodGuessDisplayTime);

						}

						if (firstBadEvaluation > -1)
						{
							// show the first bad evaluation
							BackgroundWorker.ReportProgress(0, new ComputerPlayInformation(firstBadEvaluation, CellMark.CompareFalse));
							System.Threading.Thread.Sleep(MyMasterMindBoarViewConstants.BadGuessDisplayTime);
							BackgroundWorker.ReportProgress(0, new ComputerPlayInformation(firstBadEvaluation, CellMark.None));
							System.Threading.Thread.Sleep(MyMasterMindBoarViewConstants.BadGuessDisplayTime);
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


		private void ExecuteComputerCommand(MyMasterMindCommands command)
		{
            ComputerCommand = command;
			ClearBoard();
			EnableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.Cancel });
			DisableCommands(new List<MyMasterMindCommands>() { 
				MyMasterMindCommands.ComputerSlow, MyMasterMindCommands.ComputerFast, 
				MyMasterMindCommands.User,         MyMasterMindCommands.Clear, 
				MyMasterMindCommands.Check });

			Game = new MyMasterMindGame();
            BackgroundWorker = new BackgroundWorker {WorkerReportsProgress = true};
            BackgroundWorker.DoWork += BackGroundComputerDoWork;
			BackgroundWorker.RunWorkerCompleted += BackGroundComputerCompleted;
			BackgroundWorker.ProgressChanged += BackgroundWorkerComputerProgressChanged;
			BackgroundWorker.WorkerSupportsCancellation = true;
			BackgroundWorker.RunWorkerAsync(this);
		}

		private void ComputerSlowCommand(object sender, EventArgs e)
        {
            UserPlaying = false;
            ExecuteComputerCommand(MyMasterMindCommands.ComputerSlow);
		}

		private void ComputerFastCommand(object sender, EventArgs e)
		{
			UserPlaying = false;
            ExecuteComputerCommand(MyMasterMindCommands.ComputerFast);
		}
		#endregion

		private void CancelCommand(object sender, EventArgs e)
		{
			UserPlaying = false;
            BackgroundWorker?.CancelAsync();
        }

		private void UserCommand(object sender, EventArgs e)
		{
			ClearBoard();

			EnableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.Clear });
			DisableCommands(new List<MyMasterMindCommands>() { 
				MyMasterMindCommands.ComputerSlow, MyMasterMindCommands.ComputerFast, 
				MyMasterMindCommands.User,         MyMasterMindCommands.Cancel,
			    MyMasterMindCommands.Check});

			Game = new MyMasterMindGame();

			MasterMindBoard.MarkGuessCell(0, CellMark.ForInput);

			UserPlaying = true;
		}

		private void CheckCheckCommand(object sender, EventArgs e)
        {
			bool state = UserPlaying;
			MyMasterMindCodeColors[] code = new MyMasterMindCodeColors[MyMasterMindConstants.Columns];

			if (Game != null)
			{
				int currentGuessRow = Game.GetCurrentGuessRow() + 1;
				for (int i = 0; i < MyMasterMindConstants.Columns; i++)
				{
					code[i] = MasterMindBoard.GetGuessColor(currentGuessRow, i);
				}

				state = state && (code.All(c => c != MyMasterMindCodeColors.None));
			}

			MasterMindCommands.SetButtonState(MyMasterMindCommands.Check, state);
		}

		private void CheckCommand(object sender, EventArgs e)
		{
			MyMasterMindCodeColors[] code = new MyMasterMindCodeColors[MyMasterMindConstants.Columns];
		
			int currentGuessRow = Game.GetCurrentGuessRow()+1;
			for (int i = 0; i < MyMasterMindConstants.Columns; i++ )
			{
				code[i] = MasterMindBoard.GetGuessColor(currentGuessRow, i);
			}

			IMasterMindGuessModel guess = Game.SetGuess(currentGuessRow, code);
			MasterMindBoard.SetGuessEvaluation(currentGuessRow, guess.GetEvaluation().Black, guess.GetEvaluation().White);

			MasterMindBoard.MarkGuessCell(currentGuessRow, CellMark.None);
			currentGuessRow++;
			if ( (currentGuessRow >= MyMasterMindConstants.Rows) || Game.Finished() )
			{
				ShowCode();
				DisableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.Check, MyMasterMindCommands.Cancel });
				EnableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.ComputerSlow, MyMasterMindCommands.ComputerFast, MyMasterMindCommands.User, MyMasterMindCommands.Clear });
				return;
			}
			MasterMindBoard.MarkGuessCell(currentGuessRow, CellMark.ForInput );

			MasterMindCommands.SetButtonState(MyMasterMindCommands.Check, false);
		}
		#endregion
	}
}
