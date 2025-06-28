using System.Windows.Input;

namespace RouterPlus.Core;

public class RelayCommand(Action<object> execute, Predicate<object>? canExecute = null) : ICommand
{
    private readonly Action<object> _execute = execute ?? throw new ArgumentNullException(nameof(execute));

    public bool CanExecute(object? parameter)
    {
        return canExecute?.Invoke(parameter) ?? true;
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }

    public event EventHandler? CanExecuteChanged;
}