using System;
using System.Windows.Input;

namespace ActiveClub.Firma.Helper
{
    internal class BaseCommand : ICommand
    {
        private readonly Action _command;
        private readonly Func<bool>? _canExecute;

        public BaseCommand(Action command, Func<bool>? canExecute = null)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _canExecute = canExecute;
        }

        public void Execute(object? parameter)
        {
            _command();
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute();
        }

       
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}