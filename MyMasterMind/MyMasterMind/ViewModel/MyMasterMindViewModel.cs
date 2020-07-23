﻿using MyMasterMind.Controls;
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
				MasterMindBoard.SetGuessEvaluation(currentGuessRow, guess.GetEvaluation().Black, guess.GetEvaluation().White);

				MasterMindBoard.MarkGuessCell(currentGuessRow, (CellMark)e.UserState);
			}
		}

		void BackGroundComputerDoWork(object sender, DoWorkEventArgs e)
		{
			ShowCode();

			BackgroundWorker.ReportProgress(0, CellMark.None);

			for (int i = 0; i < MyMasterMindConstants.ROWS; i++)
			{
				Game.GetNewGuess();
				for (int j = 0; j < 3; j++)
				{
					BackgroundWorker.ReportProgress(0, CellMark.CompareTrue);
					System.Threading.Thread.Sleep(100);
					BackgroundWorker.ReportProgress(0, CellMark.CompareFalse);
					System.Threading.Thread.Sleep(100);
				}

				BackgroundWorker.ReportProgress(0, CellMark.None);
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
				MasterMindCommands.EnableButton(MyMasterMindCommands.User);
				MasterMindCommands.EnableButton(MyMasterMindCommands.Computer);
				MasterMindCommands.DisableButton(MyMasterMindCommands.Check);
				return;
			}
			MasterMindBoard.MarkGuessCell(currentGuessRow, CellMark.ForInput ); 
		}
		#endregion
	}
}
