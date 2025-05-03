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
    private Dictionary<ViewType, ViewModelBase> viewModels;

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
    public ICommand LogoutCommand { get; }

    public MainWindowViewModel()
    {
        _routerService = new RouterService();
        CurrentView = new LoginViewModel(this, _routerService);
        ChangeViewCommand = new RelayCommand(ChangeView);
        LogoutCommand = new RelayCommand(Logout);
        IsLoggedIn = false;
        UpdateCurrentView();
        InitializeViews();
    }

    private void Logout(object parameter)
    {
        IsLoggedIn = false;
        _routerService.Logout();
    }

    private void ChangeView(object newViewsType)
    {
        CurrentView = viewModels.TryGetValue((ViewType)newViewsType, out var result) ?
            result :
            CurrentView;
    }

    private void InitializeViews()
    {
        viewModels = new Dictionary<ViewType, ViewModelBase>{
            { ViewType.MAIN, new MainViewModel()},
            { ViewType.SMS, new SmsViewModel(_routerService)},
            { ViewType.AutomationRules, new AutomationRulesViewModel()}
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
}