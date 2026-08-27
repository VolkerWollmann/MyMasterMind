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
			ShowStatus,     // show a status text in the command panel
			MarkOrigin,     // mark a single field of the current guess with its gene origin
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
			public string Status { get; private set; }
			public GeneticGeneOrigin Origin { get; private set; }

			public static ComputerPlayInformation MarkRow(int row, CellMark mark)
				=> new ComputerPlayInformation { Action = ComputerPlayAction.MarkRow, Row = row, Mark = mark };

			public static ComputerPlayInformation MarkField(int row, int column, MyMasterMindEvaluationColors contribution)
				=> new ComputerPlayInformation { Action = ComputerPlayAction.MarkField, Row = row, Column = column, Contribution = contribution };

			public static ComputerPlayInformation UnmarkField(int row, int column)
				=> new ComputerPlayInformation { Action = ComputerPlayAction.UnmarkField, Row = row, Column = column };

			public static ComputerPlayInformation ShowEvaluation(int black, int white)
				=> new ComputerPlayInformation { Action = ComputerPlayAction.ShowEvaluation, Black = black, White = white };

			public static ComputerPlayInformation ShowStatus(string status)
				=> new ComputerPlayInformation { Action = ComputerPlayAction.ShowStatus, Status = status };

			public static ComputerPlayInformation MarkOrigin(int row, int column, GeneticGeneOrigin origin)
				=> new ComputerPlayInformation { Action = ComputerPlayAction.MarkOrigin, Row = row, Column = column, Origin = origin };
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
			MasterMindCommands.SetCommandEventHandler(MyMasterMindCommands.ComputerStep, ComputerStepCommand);
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
			MasterMindCommands.SetStatusText(string.Empty);
			DisableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.Check, MyMasterMindCommands.Cancel});
			EnableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.ComputerSlow, MyMasterMindCommands.ComputerFast, MyMasterMindCommands.ComputerStep, MyMasterMindCommands.User, MyMasterMindCommands.Clear });
		}

		#region Computer Command
		MyMasterMindCommands ComputerCommand;
		MyMasterMindStrategy Strategy;
		BackgroundWorker BackgroundWorker;

		// signaled by the step button while a single-step run is in progress
		readonly System.Threading.AutoResetEvent StepEvent = new System.Threading.AutoResetEvent(false);

		/// <summary>
		/// Block the worker until the user clicks the step button.
		/// Returns false when the run was cancelled instead.
		/// </summary>
		private bool WaitForStep()
		{
			while (!StepEvent.WaitOne(100))
			{
				if (BackgroundWorker.CancellationPending)
					return false;
			}

			return true;
		}

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

					case ComputerPlayAction.ShowStatus:
						MasterMindCommands.SetStatusText(computerPlayInformation.Status);
						break;

					case ComputerPlayAction.MarkOrigin:
						MasterMindBoard.MarkGuessFieldOrigin(computerPlayInformation.Row, computerPlayInformation.Column, computerPlayInformation.Origin);
						break;
				}
			}
		}

		private void BackGroundComputerDoWork(object sender, DoWorkEventArgs e)
		{
			ShowCode();

			for (int i = 0; i < MyMasterMindConstants.Rows; i++)
			{
				if (Strategy == MyMasterMindStrategy.Genetic)
				{
					if (!PlayGeneticRow())
						return;
				}
				else if (ComputerCommand == MyMasterMindCommands.ComputerSlow || ComputerCommand == MyMasterMindCommands.ComputerStep)
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

						// single step: wait for the next click before trying the next candidate
						if (ComputerCommand == MyMasterMindCommands.ComputerStep && firstBadEvaluation > -1)
						{
							if (!WaitForStep())
								return;
						}

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

			if (Strategy == MyMasterMindStrategy.Genetic)
			{
				BackgroundWorker.ReportProgress(0, ComputerPlayInformation.ShowStatus(Game.Finished()
					? $"Solved in row {Game.GetCurrentGuessRow() + 1}."
					: $"Failed: code not found within {MyMasterMindConstants.Rows} rows."));
			}
		}

		/// <summary>
		/// Play one row with the genetic strategy. In slow mode the best individual
		/// of every generation is shown in the current row together with a status
		/// text. Returns false when the user cancelled.
		/// </summary>
		private bool PlayGeneticRow()
		{
			Game.StartGeneticGuess();
			int row = Game.GetCurrentGuessRow();

			bool finished;
			do
			{
				finished = Game.GeneticStep();

				if (ComputerCommand == MyMasterMindCommands.ComputerSlow || ComputerCommand == MyMasterMindCommands.ComputerStep)
				{
					IGeneticGenerationInfo info = Game.GetGeneticGenerationInfo();
					BackgroundWorker.ReportProgress(0, ComputerPlayInformation.ShowStatus(
						$"Row {row + 1}: generation {info.Generation}, best fitness {info.BestFitness}"
						+ DescribeRecombination(info)));

					bool marked = MarkBestOrigins(row, info);

					// single step: keep the generation on display until the next click
					bool cancelled = false;
					if (ComputerCommand == MyMasterMindCommands.ComputerStep)
						cancelled = !WaitForStep();
					else
						System.Threading.Thread.Sleep(MyMasterMindBoarViewConstants.GenerationDisplayTime);

					if (marked)
						UnmarkAllFields(row);
					if (cancelled)
						return false;
				}

				if (BackgroundWorker.CancellationPending)
					return false;

			} while (!finished);

			Game.CommitGeneticGuess();

			IGeneticGenerationInfo result = Game.GetGeneticGenerationInfo();
			if (!result.Consistent)
			{
				BackgroundWorker.ReportProgress(0, ComputerPlayInformation.ShowStatus(
					$"Row {Game.GetCurrentGuessRow() + 1}: no consistent code within {result.Generation} generations, playing best individual."));
			}

			if (ComputerCommand == MyMasterMindCommands.ComputerFast)
				System.Threading.Thread.Sleep(500);

			return true;
		}

		/// <summary>
		/// Textual description of how the best individual was created, matching the
		/// field marks: blue = first parent, orange = second parent, red = mutation.
		/// </summary>
		private static string DescribeRecombination(IGeneticGenerationInfo info)
		{
			if (info.BestCrossoverPoint == 0)
			{
				return info.BestOrigins[0] == GeneticGeneOrigin.Carried
					? "\nbest carried over unchanged"
					: "\nrandom start individual";
			}

			string text = $"\ncrossover after position {info.BestCrossoverPoint} (blue | orange)";

			List<int> mutations = new List<int>();
			for (int i = 0; i < MyMasterMindConstants.Columns; i++)
			{
				if (info.BestOrigins[i] == GeneticGeneOrigin.Mutation)
					mutations.Add(i + 1);
			}
			if (mutations.Count > 0)
				text += $", mutation at {string.Join(",", mutations)} (red)";

			return text;
		}

		/// <summary>
		/// Mark the fields of the best individual with the origin of their genes.
		/// Only bred individuals are marked; returns whether marks were set.
		/// </summary>
		private bool MarkBestOrigins(int row, IGeneticGenerationInfo info)
		{
			if (info.BestCrossoverPoint == 0)
				return false;

			for (int column = 0; column < MyMasterMindConstants.Columns; column++)
				BackgroundWorker.ReportProgress(0, ComputerPlayInformation.MarkOrigin(row, column, info.BestOrigins[column]));

			return true;
		}

		private void UnmarkAllFields(int row)
		{
			for (int column = 0; column < MyMasterMindConstants.Columns; column++)
				BackgroundWorker.ReportProgress(0, ComputerPlayInformation.UnmarkField(row, column));
		}

		private void BackGroundComputerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			DisableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.Check, MyMasterMindCommands.Cancel });
			EnableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.ComputerSlow, MyMasterMindCommands.ComputerFast, MyMasterMindCommands.ComputerStep, MyMasterMindCommands.User, MyMasterMindCommands.Clear });
		}


		private void ExecuteComputerCommand(MyMasterMindCommands command)
		{
            ComputerCommand = command;
			Strategy = MasterMindCommands.GetSelectedStrategy();
			MasterMindCommands.SetStatusText(string.Empty);
			StepEvent.Reset();
			ClearBoard();
			EnableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.Cancel });
			DisableCommands(new List<MyMasterMindCommands>() {
				MyMasterMindCommands.ComputerSlow, MyMasterMindCommands.ComputerFast,
				MyMasterMindCommands.ComputerStep,
				MyMasterMindCommands.User,         MyMasterMindCommands.Clear,
				MyMasterMindCommands.Check });

			// in single-step mode the step button stays enabled: it advances the run
			if (command == MyMasterMindCommands.ComputerStep)
				EnableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.ComputerStep });

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

		private void ComputerStepCommand(object sender, EventArgs e)
		{
			// while a single-step run is in progress the button advances it by one step
			if (BackgroundWorker != null && BackgroundWorker.IsBusy && ComputerCommand == MyMasterMindCommands.ComputerStep)
			{
				StepEvent.Set();
				return;
			}

			UserPlaying = false;
			ExecuteComputerCommand(MyMasterMindCommands.ComputerStep);
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
			MasterMindCommands.SetStatusText(string.Empty);

			EnableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.Clear });
			DisableCommands(new List<MyMasterMindCommands>() {
				MyMasterMindCommands.ComputerSlow, MyMasterMindCommands.ComputerFast,
				MyMasterMindCommands.ComputerStep,
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
				EnableCommands(new List<MyMasterMindCommands>() { MyMasterMindCommands.ComputerSlow, MyMasterMindCommands.ComputerFast, MyMasterMindCommands.ComputerStep, MyMasterMindCommands.User, MyMasterMindCommands.Clear });
				return;
			}
			MasterMindBoard.MarkGuessCell(currentGuessRow, CellMark.ForInput );

			MasterMindCommands.SetButtonState(MyMasterMindCommands.Check, false);
		}
		#endregion
	}
}
