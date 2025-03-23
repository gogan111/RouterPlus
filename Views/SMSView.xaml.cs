using RouterPlus.ViewModels;

namespace RouterPlus.Views;

public partial class SMSView
{
    public SMSView()
    {
        InitializeComponent();
        Loaded += async (s, e) => await ((SmsViewModel)DataContext).LoadSmsListAsync();
    }
}