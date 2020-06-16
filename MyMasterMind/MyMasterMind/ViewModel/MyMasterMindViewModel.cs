using MyMasterMind.Controls;
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
		}
	}
}
