using System.Windows;
using System.Windows.Controls;
using RouterPlus.Services;
using RouterPlus.ViewModels;

namespace RouterPlus.Views;

public partial class LoginView : UserControl
{
    public LoginView(MainWindowViewModel mainWindowViewModel, RouterService routerService)
    {
        InitializeComponent();
        this.DataContext = new LoginViewModel(mainWindowViewModel, routerService);
    }

    public LoginView()
    {
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel viewModel)
        {
            viewModel.Password = ((PasswordBox)sender).Password;
        }
    }
}