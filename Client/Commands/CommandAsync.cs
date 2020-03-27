using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.Commands
{
    public class CommandAsync : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Predicate<object> _canExecute;
        private bool isExecuting;

        public CommandAsync(Func<Task> execute) : this(execute, null) { }

        public CommandAsync(Func<Task> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (!isExecuting && _canExecute == null) return true;
            return (!isExecuting && _canExecute(parameter));
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public async void Execute(object parameter)
        {
            isExecuting = true;
            try { await _execute(); }
            finally { isExecuting = false; }
        }
    }
}
