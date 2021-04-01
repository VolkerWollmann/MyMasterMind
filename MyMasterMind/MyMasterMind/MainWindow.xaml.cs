using MyMasterMind.ViewModel;

namespace MyMasterMind
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
    {
		public MainWindow()
		{
			InitializeComponent();
			ViewModelInitialize.InitializeViewModels(MyMasterMindGrid);
		}
	}
}
