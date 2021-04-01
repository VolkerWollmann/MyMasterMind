using System;

namespace MyMasterMind.Interfaces
{
	public interface IMasterMindCommandView
	{
		void SetCommandEventHandler(MyMasterMindCommands command, EventHandler eventHandler);

		void RaiseCommandEventHandler(MyMasterMindCommands command);

		void SetButtonState(MyMasterMindCommands command, bool state);

		bool GetButtonState(MyMasterMindCommands command);

	}
}
