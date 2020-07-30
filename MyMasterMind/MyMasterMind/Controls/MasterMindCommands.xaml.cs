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
		private Dictionary<MyMasterMindCommands, EventHandler> EventHandler;
		public MasterMindCommands()
		{
			InitializeComponent();
			EventHandler = new Dictionary<MyMasterMindCommands, EventHandler>();
		}

		#region Command event handler registration
		public void SetCommandEventHandler(MyMasterMindCommands command, EventHandler eventHandler)
		{
			if (EventHandler.ContainsKey(command))
				EventHandler[command] += eventHandler;
			else
				EventHandler.Add(command, eventHandler);
		}

		#endregion

		#region Command event operation

		private void ButtonCommandComputerFast_Click(object sender, RoutedEventArgs e)
		{
			EventHandler[MyMasterMindCommands.ComputerFast]?.Invoke(sender, e);
		}

		private void ButtonCommandComputerSlow_Click(object sender, RoutedEventArgs e)
		{
			EventHandler[MyMasterMindCommands.ComputerSlow]?.Invoke(sender, e);
		}


		private void ButtonCommandClear_Click(object sender, RoutedEventArgs e)
		{
			EventHandler[MyMasterMindCommands.Clear]?.Invoke(sender, e);
		}

		private void ButtonCommandUser_Click(object sender, RoutedEventArgs e)
		{
			EventHandler[MyMasterMindCommands.User]?.Invoke(sender, e);
		}

		private void ButtonCommandCheck_Click(object sender, RoutedEventArgs e)
		{
			EventHandler[MyMasterMindCommands.Check]?.Invoke(sender, e);
		}

		private void ButtonCommandCancel_Click(object sender, RoutedEventArgs e)
		{
			EventHandler[MyMasterMindCommands.Cancel]?.Invoke(sender, e);
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

				case MyMasterMindCommands.ComputerFast:
					ButtonCommandComputerFast.IsEnabled = state;
					break;

				case MyMasterMindCommands.ComputerSlow:
					ButtonCommandComputerSlow.IsEnabled = state;
					break;

				case MyMasterMindCommands.User:
					ButtonCommandUser.IsEnabled = state;
					break;

				case MyMasterMindCommands.Cancel:
					ButtonCommandCancel.IsEnabled = state;
					break;

				case MyMasterMindCommands.Clear:
					ButtonCommandClear.IsEnabled = state;
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
