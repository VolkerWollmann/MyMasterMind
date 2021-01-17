using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyMasterMind.Interfaces
{
	interface IMasterMindCommandView
	{
		void SetCommandEventHandler(MyMasterMindCommands command, EventHandler eventHandler);

		void EnableButton(MyMasterMindCommands command);

		void DisableButton(MyMasterMindCommands command);

	}
}
