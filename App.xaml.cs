using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;

namespace RouterPlus;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-EN");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-EN");
    }
}