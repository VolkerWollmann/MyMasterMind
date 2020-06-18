using MyMasterMind.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyMasterMind.Controls
{
	/// <summary>
	/// Interaction logic for MasterMindCommands.xaml
	/// </summary>
	public partial class MasterMindCommands : UserControl, IMasterMindCommandView
	{
		private EventHandler ClearCommandEventHandler;
		private EventHandler ComputerCommandEventHandler;
		public MasterMindCommands()
		{
			InitializeComponent();
		}

		public void SetClearCommandEventHandler(EventHandler eventHandler)
		{
			ClearCommandEventHandler += eventHandler;
		}

		public void SetComputerCommandEventHandler(EventHandler eventHandler)
		{
			ComputerCommandEventHandler += eventHandler;
		}

		private void ButtonCommandComputer_Click(object sender, RoutedEventArgs e)
		{
			if (ComputerCommandEventHandler != null)
				ComputerCommandEventHandler(sender, e);
		}

		private void ButtonCommandClear_Click(object sender, RoutedEventArgs e)
		{
			if (ClearCommandEventHandler != null)
				ClearCommandEventHandler(sender, e);
		}
	}
}
