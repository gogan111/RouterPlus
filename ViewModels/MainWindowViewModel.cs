using System.Windows.Input;
using RouterPlus.Core;
using RouterPlus.Views;

namespace RouterPlus.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private bool _isLoggedIn;
    private object _currentView;

    public bool IsLoggedIn
    {
        get => _isLoggedIn;
        set
        {
            _isLoggedIn = value;
            OnPropertyChanged();
            UpdateCurrentView();
        }
    }

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
    public ICommand LogoutCommand { get; }

    public MainWindowViewModel()
    {
        CurrentView = new LoginViewModel(this);
        ChangeViewCommand = new RelayCommand(ChangeView);
        LogoutCommand = new RelayCommand(Logout);
        IsLoggedIn = false;
        UpdateCurrentView();
    }
    
    private void Logout(object parameter)
    {
        IsLoggedIn = false;
    }

    private void ChangeView(object newViews)
    {
        var viewName = newViews.ToString();
        CurrentView = viewName switch
        {
            "Main" => new MainViewModel(),
            "SMS" => new SMSViewModel(),
            "AutomationRules" => new AutomationRulesViewModel(),
            _ => CurrentView
        };
    }

    private void UpdateCurrentView()
    {
        if (IsLoggedIn)
        {
            CurrentView = new MainView();
        }
        else
        {
            CurrentView = new LoginView(this);
        }
    }

    private void GoBack(object parameter)
    {
        CurrentView = this;
    }
}