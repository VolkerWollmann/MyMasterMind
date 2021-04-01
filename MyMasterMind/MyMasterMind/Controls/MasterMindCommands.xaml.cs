using MyMasterMind.Interfaces;
using MyMasterMind.Commands;
using System;
using System.Collections.Generic;

namespace MyMasterMind.Controls
{
	/// <summary>
	/// Interaction logic for MasterMindCommands.xaml
	/// </summary>
	public partial class MasterMindCommands : IMasterMindCommandView
	{
		private readonly Dictionary<MyMasterMindCommands, EventHandler> EventHandler;
		private readonly bool[] State = new bool[6];
		public MasterMindCommands()
		{
			InitializeComponent();
			EventHandler = new Dictionary<MyMasterMindCommands, EventHandler>();

			this.ButtonCommandCancel.Command = new ButtonCommand(this, MyMasterMindCommands.Cancel);
			this.ButtonCommandCheck.Command = new ButtonCommand(this, MyMasterMindCommands.Check);
			this.ButtonCommandClear.Command = new ButtonCommand(this, MyMasterMindCommands.Clear);
			this.ButtonCommandComputerFast.Command = new ButtonCommand(this, MyMasterMindCommands.ComputerFast);
			this.ButtonCommandComputerSlow.Command = new ButtonCommand(this, MyMasterMindCommands.ComputerSlow);
			this.ButtonCommandUser.Command = new ButtonCommand(this, MyMasterMindCommands.User);
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
        public void RaiseCommandEventHandler(MyMasterMindCommands command)
		{
			if (EventHandler.ContainsKey(command))
				EventHandler[command].Invoke(this,null);
		}

		#endregion

		#region Sufrace/Appearance Operation
		private void SetButton(MyMasterMindCommands command, bool state)
		{
			State[(int)command] = state;
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

		public bool GetButtonState(MyMasterMindCommands command)
		{
			return State[(int)command];
		}

		public void SetButtonState(MyMasterMindCommands command, bool state)
		{
			SetButton(command, state);
		}

        #endregion

    }
}
