using System.Windows.Input;
using RouterPlus.Core;
using RouterPlus.Services;
using RouterPlus.Views;

namespace RouterPlus.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private bool _isLoggedIn;
    private object _currentView;
    private readonly RouterService _routerService;

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
        _routerService = new RouterService();
        CurrentView = new LoginViewModel(this, _routerService);
        ChangeViewCommand = new RelayCommand(ChangeView);
        LogoutCommand = new RelayCommand(Logout);
        IsLoggedIn = false;
        UpdateCurrentView();
    }

    private void Logout(object parameter)
    {
        IsLoggedIn = false;
        _routerService.Logout();
    }

    private void ChangeView(object newViews)
    {
        var viewName = newViews.ToString();
        CurrentView = viewName switch
        {
            "Main" => new MainViewModel(),
            "SMS" => new SmsViewModel(_routerService),
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
            CurrentView = new LoginView(this, _routerService);
        }
    }

    private void GoBack(object parameter)
    {
        CurrentView = this;
    }
}