using MyMasterMind.Interfaces;
using System;
using System.Windows.Input;

namespace MyMasterMind.Commands
{
    public class ButtonCommand : ICommand
    {
        readonly IMasterMindCommandView MasterMindCommandView;
        readonly MyMasterMindCommands Command;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
            //return MasterMindCommandView.GetButtonState(Command);
        }

        public void Execute(object parameter)
        {
            MasterMindCommandView.RaiseCommandEventHandler(Command);
        }

        public ButtonCommand(IMasterMindCommandView masterMindCommandView,
            MyMasterMindCommands command)
        {
            MasterMindCommandView = masterMindCommandView;
            Command = command;
        }
    }
}
