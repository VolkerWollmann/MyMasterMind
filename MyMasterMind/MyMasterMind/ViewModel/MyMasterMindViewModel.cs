using MyMasterMind.Controls;
using MyMasterMind.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMasterMind.ViewModel
{
	public class MyMasterMindViewModel
	{
		MasterMindBoard MasterMindBoard;
		MasterMindCommands MasterMindCommands;



		public MyMasterMindViewModel(MasterMindBoard masterMindBoard, MasterMindCommands masterMindCommands)
		{
			MasterMindBoard = masterMindBoard;
			MasterMindCommands = masterMindCommands;

			Random random = new Random();

			for(int i=0; i<10; i++)
			{
				for(int j =0; j<4; j++)
				{
					masterMindBoard.SetColor(i, j, (MyMasterMindColors)random.Next(3, 9));
				}
			}

			masterMindBoard.SetEvaluation(0, 0, 0);
			masterMindBoard.SetEvaluation(1, 0, 1);
			masterMindBoard.SetEvaluation(2, 0, 2);
			masterMindBoard.SetEvaluation(3, 0, 3);
			masterMindBoard.SetEvaluation(4, 0, 4);
			masterMindBoard.SetEvaluation(5, 1, 0);
			masterMindBoard.SetEvaluation(6, 2, 0);
			masterMindBoard.SetEvaluation(7, 3, 0);
			masterMindBoard.SetEvaluation(8, 4, 0);
			masterMindBoard.SetEvaluation(9, 3, 1);
		}
	}
}
