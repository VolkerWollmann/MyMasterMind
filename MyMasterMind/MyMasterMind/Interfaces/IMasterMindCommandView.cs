using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMasterMind.Interfaces
{
	interface IMasterMindCommandView
	{
		void SetClearCommandEventHandler(EventHandler eventHandler);
		void SetComputerCommandEventHandler(EventHandler eventHandler);
	}
}
