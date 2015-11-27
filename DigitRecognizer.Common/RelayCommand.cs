using System;
using System.Windows.Input;

namespace DigitRecognizer.Common
{
    public sealed class RelayCommand : ICommand
    {
        private readonly Func<object, bool> _canExecuteDelegate;
        private readonly Action<object> _action;

        public RelayCommand(Action<object> action)
            : this(action, _ => true)
        {
            Guard.NotNull(action, nameof(action));
            _action = action;
        }

        public RelayCommand(Action<object> action, Func<object, bool> canExecute)
        {
            Guard.NotNull(action, nameof(action));
            Guard.NotNull(canExecute, nameof(canExecute));
            _canExecuteDelegate = canExecute;
            _action = action;
        }

        public bool CanExecute(object parameter) => _canExecuteDelegate(parameter);

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
