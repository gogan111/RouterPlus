using System.Windows.Input;
using RouterPlus.Core;

namespace RouterPlus.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private object _currentView;

    public object CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }
    
    public ICommand ChangeViewCommand { get; }
    public ICommand BackCommand { get; }

    public MainWindowViewModel()
    {
        ChangeViewCommand = new RelayCommand(ChangeView);
        BackCommand = new RelayCommand(GoBack);
        // Set the initial view
        CurrentView = new LoginViewModel();
    }
    
    private void ChangeView(object newViews)
    {
        var viewName = newViews.ToString();
        CurrentView = viewName switch
        {
            "Main" => new MainViewModel(),
            "SMS" => new SMSViewModel(),
            "AutomationRules" => new AutomationRulesViewModel(),
            "Login" => new LoginViewModel(),
            _ => CurrentView
        };
    }
    
    private void GoBack(object parameter)
    {
        CurrentView = new LoginViewModel();
    }

}