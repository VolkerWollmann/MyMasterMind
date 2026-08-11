using MyMasterMind.Interfaces;
using MyMasterMind.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MyMasterMind.ViewModel
{
	public class MyMasterMindViewModel
	{
		private enum ComputerPlayAction
		{
			MarkRow,        // mark a whole guess row (green/red flash)
			MarkField,      // mark a single field of the current guess with its contribution
			UnmarkField,    // remove the mark from a single field of the current guess
			ShowEvaluation, // show black/white counts in the current guess row
		}

		private class ComputerPlayInformation
		{
			public ComputerPlayAction Action { get; private set; }
			public int Row { get; private set; }
			public CellMark Mark { get; private set; }
			public int Column { get; private set; }
			public MyMasterMindEvaluationColors Contribution { get; private set; }
			public int Black { get; private set; }
			public int White { get; private set; }

			public static ComputerPlayInformation MarkRow(int row, CellMark mark)
				=> new ComputerPlayInformation { Action = ComputerPlayAction.MarkRow, Row = row, Mark = mark };

			public static ComputerPlayInformation MarkField(int row, int column, MyMasterMindEvaluationColors contribution)
				=> new ComputerPlayInformation { Action = ComputerPlayAction.MarkField, Row = row, Column = column, Contribution = contribution };

			public static ComputerPlayInformation UnmarkField(int row, int column)
				=> new ComputerPlayInformation { Action = ComputerPlayAction.UnmarkField, Row = row, Column = column };

			public static ComputerPlayInformation ShowEvaluation(int black, int white)
				=> new ComputerPlayInformation { Action = ComputerPlayAction.ShowEvaluation, Black = black, White = white };
		}

        readonly IMasterMindBoardView MasterMindBoard;
        readonly IMasterMindCommandView MasterMindCommands;
		IMasterMindGameModel Game;
		bool UserPlaying;

		private void ClearBoard()
		{
			MasterMindBoard.Clear();
		}

		private void ShowCode()
		{
            MasterMindBoard.SetCode(Game.GetCode().Colors);
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

			((ISetEnableCheckCommandEventHandler)MasterMindBoard).SetEnableCheckCommandEventHandler(EnableCheckCommand);

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
			int currentGuessRow = Game.GetCurrentGuessRow();
			if (guess != null)
			{
                MasterMindBoard.SetGuessColors(currentGuessRow, guess.GetCode().Colors);

				if (e.UserState == null && guess.GetEvaluation() != null )
					MasterMindBoard.SetGuessEvaluation(currentGuessRow, guess.GetEvaluation().Black, guess.GetEvaluation().White);
			}

			if (e.UserState is ComputerPlayInformation computerPlayInformation)
			{
				switch (computerPlayInformation.Action)
				{
					case ComputerPlayAction.MarkRow:
						MasterMindBoard.MarkGuessCell(computerPlayInformation.Row, computerPlayInformation.Mark);
						break;

					case ComputerPlayAction.MarkField:
						MasterMindBoard.MarkGuessField(computerPlayInformation.Row, computerPlayInformation.Column, computerPlayInformation.Contribution);
						break;

					case ComputerPlayAction.UnmarkField:
						MasterMindBoard.UnmarkGuessField(computerPlayInformation.Row, computerPlayInformation.Column);
						break;

					case ComputerPlayAction.ShowEvaluation:
						MasterMindBoard.SetGuessEvaluation(currentGuessRow, computerPlayInformation.Black, computerPlayInformation.White);
						break;
				}
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

						// show evaluations descending
						int jMax = Game.GetCurrentGuessRow();

						for (int j = jMax-1; j > firstBadEvaluation; j--)
						{
							// show the good evaluation
							BackgroundWorker.ReportProgress(0, ComputerPlayInformation.MarkRow(j, CellMark.CompareTrue));
							System.Threading.Thread.Sleep(MyMasterMindBoarViewConstants.GoodGuessDisplayTime);

							// step through the fields of the current guess and show which one
							// counts as a black evaluation, which one as a white evaluation
							// and which one does not contribute against the green row;
							// mark the matched field of the green row at the same time
							MyMasterMindComparisonDetail[] comparisonDetails = Game.GetComparisonDetails(j);
							int black = 0;
							int white = 0;
							BackgroundWorker.ReportProgress(0, ComputerPlayInformation.ShowEvaluation(black, white));
							for (int k = 0; k < MyMasterMindConstants.Columns; k++)
							{
								MyMasterMindComparisonDetail detail = comparisonDetails[k];

								if (detail.Contribution == MyMasterMindEvaluationColors.Black)
									black++;
								else if (detail.Contribution == MyMasterMindEvaluationColors.White)
									white++;

								BackgroundWorker.ReportProgress(0, ComputerPlayInformation.MarkField(jMax, k, detail.Contribution));
								if (detail.OtherColumn >= 0)
									BackgroundWorker.ReportProgress(0, ComputerPlayInformation.MarkField(j, detail.OtherColumn, detail.Contribution));
								BackgroundWorker.ReportProgress(0, ComputerPlayInformation.ShowEvaluation(black, white));
								System.Threading.Thread.Sleep(MyMasterMindBoarViewConstants.GoodGuessDisplayTime);
								BackgroundWorker.ReportProgress(0, ComputerPlayInformation.UnmarkField(jMax, k));
								if (detail.OtherColumn >= 0)
									BackgroundWorker.ReportProgress(0, ComputerPlayInformation.UnmarkField(j, detail.OtherColumn));

								if (BackgroundWorker.CancellationPending)
								{
									BackgroundWorker.ReportProgress(0, ComputerPlayInformation.MarkRow(j, CellMark.None));
									return;
								}
							}

							BackgroundWorker.ReportProgress(0, ComputerPlayInformation.MarkRow(j, CellMark.None));
							System.Threading.Thread.Sleep(MyMasterMindBoarViewConstants.GoodGuessDisplayTime);
						}

						if (firstBadEvaluation > -1)
						{
							// show the first bad evaluation and clear the black and white
							// counts shown for the rejected guess
							BackgroundWorker.ReportProgress(0, ComputerPlayInformation.MarkRow(firstBadEvaluation, CellMark.CompareFalse));
							System.Threading.Thread.Sleep(MyMasterMindBoarViewConstants.BadGuessDisplayTime);
							BackgroundWorker.ReportProgress(0, ComputerPlayInformation.MarkRow(firstBadEvaluation, CellMark.None));
							BackgroundWorker.ReportProgress(0, ComputerPlayInformation.ShowEvaluation(0, 0));
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

		private void EnableCheckCommand(object sender, EventArgs e)
        {
			bool state = UserPlaying;

            if (Game != null)
			{
				int currentGuessRow = Game.GetCurrentGuessRow() + 1;
                var code = MasterMindBoard.GetGuessColors(currentGuessRow);
				state = state && (code.All(c => c != MyMasterMindCodeColors.None));
			}

			MasterMindCommands.SetButtonState(MyMasterMindCommands.Check, state);
		}

		private void CheckCommand(object sender, EventArgs e)
		{
            int currentGuessRow = Game.GetCurrentGuessRow()+1;
            var code = MasterMindBoard.GetGuessColors(currentGuessRow);
            
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
