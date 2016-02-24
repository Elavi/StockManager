using System;
using System.Windows.Input;

namespace StockManager
{
    public class DelegateCommand : ICommand
    {
        public DelegateCommand(Action<object> executeDelegate)
        {
            this.executeDelegate = executeDelegate;
        }

        public DelegateCommand(ICommand createEquityCommand)
        {
            this.createEquityCommand = createEquityCommand;
        }

        public Action<object> executeDelegate;
        private ICommand createEquityCommand;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.executeDelegate(parameter);
        }
    }
}
