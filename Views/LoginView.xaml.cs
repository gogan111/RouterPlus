using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using RouterPlus.ViewModels;

namespace RouterPlus.Views;

public partial class LoginView : UserControl
{
    public LoginView(MainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();
        this.DataContext = new LoginViewModel(mainWindowViewModel);
    }
    
    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel viewModel)
        {
            viewModel.Password = ((PasswordBox)sender).Password;
        }
    }
}