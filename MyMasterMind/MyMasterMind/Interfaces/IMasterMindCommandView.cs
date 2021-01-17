using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
