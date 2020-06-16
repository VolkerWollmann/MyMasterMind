using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MyMasterMind.Controls;

namespace MyMasterMind.ViewModel
{
	public class ViewModelInitialize
	{		
		public static void InitializeViewModels(Grid grid)
		{
			var myMasterMindViewModel  = grid.Children.OfType<MasterMindBoard>().First();
			var myMasterMindCommands = grid.Children.OfType<MasterMindCommands>().First();
		}
	}
}
