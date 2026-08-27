using System;

namespace MyMasterMind.Interfaces
{
	public interface IMasterMindCommandView
	{
		void SetCommandEventHandler(MyMasterMindCommands command, EventHandler eventHandler);

		void RaiseCommandEventHandler(MyMasterMindCommands command);

		void SetButtonState(MyMasterMindCommands command, bool state);

		bool GetButtonState(MyMasterMindCommands command);

		/// <summary>
		/// Strategy the computer shall use for its guesses, as selected by the user.
		/// </summary>
		MyMasterMindStrategy GetSelectedStrategy();

		/// <summary>
		/// Show a status text, e.g. the progress of the genetic search.
		/// </summary>
		void SetStatusText(string text);
	}
}
