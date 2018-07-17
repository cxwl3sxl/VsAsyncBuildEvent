using System;
using System.Windows.Input;

namespace VsAsyncBuildEvent.Model
{
    public class RealCommand<T> : ICommand where T : BuildProcess
    {
        private readonly Action<T> _action;
        public RealCommand(Action<T> action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            _action?.Invoke(parameter as T);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}