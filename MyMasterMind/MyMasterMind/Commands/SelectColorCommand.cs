using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyMasterMind.Controls;

namespace MyMasterMind.Commands
{
	public class SelectColorCommand : ICommand
	{
		private CodeField CodeField;
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			;
		}

		public SelectColorCommand(CodeField codeField )
		{
			CodeField  = codeField;
		}
	}
}
