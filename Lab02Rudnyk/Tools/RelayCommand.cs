using System.Windows.Input;

namespace Lab02Rudnyk.Tools
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            _execute = (o) => execute?.Invoke();
            _canExecute = (o) =>
            {
                if (canExecute == null)
                {
                    return true;
                }
                return canExecute.Invoke();
            };
       
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute.Invoke(parameter);
        }

        public async void Execute(object? parameter)
        {
            _execute.Invoke(parameter);
        }
    }
}
