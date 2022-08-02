using System.Linq;
using System.Windows.Controls;
using MyMasterMind.Controls;

namespace MyMasterMind.ViewModel
{
	public class ViewModelInitialize
	{
		static MyMasterMindViewModel _myMasterMindViewModel;
		public static void InitializeViewModels(Grid grid)
		{
			var myMasterMindBoard  = grid.Children.OfType<MasterMindBoard>().First();
			var myMasterMindCommands = grid.Children.OfType<MasterMindCommands>().First();

			_myMasterMindViewModel = new MyMasterMindViewModel(myMasterMindBoard, myMasterMindCommands);
			
		}
	}
}
