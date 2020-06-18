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
	using DataTriple = Tuple<int, int, int>;
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
					MasterMindBoard.SetColor(i, j, (MyMasterMindColors)random.Next(3, 9));
			}

			List<DataTriple> testEvaluations = new List<DataTriple>() {
				new DataTriple(0,0,0),
				new DataTriple(1,0,1),
				new DataTriple(2,0,2),
				new DataTriple(3,0,3),
				new DataTriple(4,0,4),
				new DataTriple(5,1,0),
				new DataTriple(6,2,0),
				new DataTriple(7,3,0),
				new DataTriple(8,4,0),
				new DataTriple(9,3,1),
			};

			testEvaluations.ForEach(e => { MasterMindBoard.SetEvaluation(e.Item1, e.Item2, e.Item3); });

		}

		private void ClearBoard()
		{
			for(int i=0; i< 10; i++)
			{
				for (int j = 0; j < 4; j++)
					MasterMindBoard.SetColor(i, j, MyMasterMindColors.Gray);

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

		private void ComputerCommand(object sender, EventArgs e)
		{
			TestBoard();
		}

		#endregion
	}
}
