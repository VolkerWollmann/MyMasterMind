using System;
using System.Windows.Input;
using MyMasterMind.Controls;
using MyMasterMind.Interfaces;

namespace MyMasterMind.Commands
{
	public class SelectColorCommand : ICommand
	{
		private readonly CodeField CodeField;
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			CodeField.SetColor((MyMasterMindCodeColors)parameter);
		}

		public SelectColorCommand(CodeField codeField )
		{
			CodeField  = codeField;
		}
	}
}
