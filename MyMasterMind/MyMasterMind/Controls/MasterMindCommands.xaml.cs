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
		private EventHandler UserCommandEventHandler;
		private EventHandler CheckCommandEventHandler;
		public MasterMindCommands()
		{
			InitializeComponent();
		}

		#region Command event handler registration
		public void SetClearCommandEventHandler(EventHandler eventHandler)
		{
			ClearCommandEventHandler += eventHandler;
		}

		public void SetComputerCommandEventHandler(EventHandler eventHandler)
		{
			ComputerCommandEventHandler += eventHandler;
		}

		public void SetUserCommandEventHandler(EventHandler eventHandler)
		{
			UserCommandEventHandler += eventHandler;
		}

		public void SetCheckCommandEventHandler(EventHandler eventHandler)
		{
			CheckCommandEventHandler += eventHandler;
		}
		#endregion

		#region Command event operation
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

		private void ButtonCommandUser_Click(object sender, RoutedEventArgs e)
		{
			if (UserCommandEventHandler != null)
				UserCommandEventHandler(sender, e);
		}

		private void ButtonCommandCheck_Click(object sender, RoutedEventArgs e)
		{
			if (CheckCommandEventHandler != null)
				CheckCommandEventHandler(sender, e);

		}

		#endregion

		#region Sufrace/Appearance Operation
		private void SetButton(MyMasterMindCommands command, bool state)
		{
			switch (command)
			{
				case MyMasterMindCommands.Check:
					ButtonCommandCheck.IsEnabled = state;
					break;

				case MyMasterMindCommands.Computer:
					ButtonCommandComputer.IsEnabled = state;
					break;

				case MyMasterMindCommands.User:
					ButtonCommandUser.IsEnabled = state;
					break;
			}
		}
		public void EnableButton(MyMasterMindCommands command)
		{
			SetButton(command, true);
		}

		public void DisableButton(MyMasterMindCommands command)
		{
			SetButton(command, false);
		}

		#endregion
	}
}
