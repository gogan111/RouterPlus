using System.Windows.Input;
using RouterPlus.Core;
using RouterPlus.Helpers;
using RouterPlus.Services;

namespace RouterPlus.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly RouterService _routerService;

    private string _url;
    private string _password;
    private string _errorMessage;
    private bool _isErrorVisible;
    private readonly MainWindowViewModel _mainWindowViewModel;

    public string Url
    {
        get => _url;
        set
        {
            _url = value;
            _isErrorVisible = false;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            _isErrorVisible = false;
            OnPropertyChanged();
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public bool IsErrorVisible
    {
        get => _isErrorVisible;
        set
        {
            _isErrorVisible = value;
            OnPropertyChanged();
        }
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel(MainWindowViewModel mainWindowViewModel, RouterService routerService)
    {
        _routerService = routerService;
        _mainWindowViewModel = mainWindowViewModel;
        LoginCommand = new RelayCommand(Login);
    }

    private async void Login(object parameter)
    {
        bool isUrlValid = ValidationHelper.ValidateField(Url, "URL", ref _errorMessage, ref _isErrorVisible);
        bool isPasswordValid =
            ValidationHelper.ValidateField(Password, "Password", ref _errorMessage, ref _isErrorVisible);

        OnPropertyChanged(nameof(ErrorMessage));
        OnPropertyChanged(nameof(IsErrorVisible));

        if (isUrlValid && isPasswordValid)
        {
            bool isAuthenticated = await _routerService.AuthenticateAsync(Url, Password);
            if (isAuthenticated)
            {
                _mainWindowViewModel.IsLoggedIn = true;
                _mainWindowViewModel.CurrentView = new MainViewModel();
            }
            else
            {
                ErrorMessage = "Invalid credentials.";
                IsErrorVisible = true;
            }
        }
    }
}