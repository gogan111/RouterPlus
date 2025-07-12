using System.ComponentModel;
using RouterPlus.Models.steps;

namespace RouterPlus.Models;

public abstract class Step : INotifyPropertyChanged
{
    private bool _isExpanded;
    private string _message;
    private string _number;

    public Step(string message, string number)
    {
        _message = message;
        _number = number;
    }

    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (_isExpanded != value)
            {
                _isExpanded = value;
                OnPropertyChanged(nameof(IsExpanded));
            }
        }
    }

    public string Message
    {
        get => _message;
        set
        {
            if (_message != value)
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
    }

    public string Number
    {
        get => _number;
        set
        {
            if (_number != value)
            {
                _number = value;
                OnPropertyChanged(nameof(Number));
            }
        }
    }

    public string Name => StepType().ToString();
    public abstract StepType StepType();
    public abstract bool Execute();

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}