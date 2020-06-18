using MyMasterMind.Controls;
using MyMasterMind.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyMasterMind.ViewModel
{
	public class MyMasterMindViewModel
	{
		MasterMindBoard MasterMindBoard;
		IMasterMindCommandView MasterMindCommands;

		private void TestBoard()
		{
			Random random = new Random();

			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					MasterMindBoard.SetColor(i, j, (MyMasterMindColors)random.Next(3, 9));
				}
			}

			MasterMindBoard.SetEvaluation(0, 0, 0);
			MasterMindBoard.SetEvaluation(1, 0, 1);
			MasterMindBoard.SetEvaluation(2, 0, 2);
			MasterMindBoard.SetEvaluation(3, 0, 3);
			MasterMindBoard.SetEvaluation(4, 0, 4);
			MasterMindBoard.SetEvaluation(5, 1, 0);
			MasterMindBoard.SetEvaluation(6, 2, 0);
			MasterMindBoard.SetEvaluation(7, 3, 0);
			MasterMindBoard.SetEvaluation(8, 4, 0);
			MasterMindBoard.SetEvaluation(9, 3, 1);
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

		}

		private void ComputerCommand(object sender, EventArgs e)
		{
			TestBoard();
		}

		#endregion
	}
}
