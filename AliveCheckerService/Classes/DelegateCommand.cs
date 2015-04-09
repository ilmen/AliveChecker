using System;
using System.Windows.Input;

namespace AliveCheckerService.Classes
{
    /// <summary> Класс делегата для команды без параметра </summary>
    public class DelegateCommand : ICommand
    {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                CommandAction();
            }
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    /// <summary> Класс делегата для команды c параметром </summary>
    public class DelegateCommand<T> : ICommand
    {
        public Action<T> CommandAction { get; set; }
        public Func<T, bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                CommandAction((T)parameter);
            }
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}