using System.Windows;
using RouterPlus.ViewModels;

namespace RouterPlus;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}