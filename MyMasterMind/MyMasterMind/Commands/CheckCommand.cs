using MyMasterMind.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyMasterMind.Commands
{
    class CheckCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        MyMasterMindViewModel _myMasterMindViewModel;
        public CheckCommand(MyMasterMindViewModel myMasterMindViewModel)
        {
            _myMasterMindViewModel = myMasterMindViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ;
        }
    }
}
