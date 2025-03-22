using System.Windows;
using System.Windows.Controls;
using RouterPlus.ViewModels;

namespace RouterPlus.Views;

public partial class SMSView : UserControl
{
    public SMSView()
    {
        InitializeComponent();
        Loaded += async (s, e) => await ((SmsViewModel)DataContext).LoadSmsListAsync();
    }
    
    private void CopyTextContextMenu(object sender, ContextMenuEventArgs e)
    {
        if (sender is TextBlock textBlock)
        {
            var contextMenu = new ContextMenu();
            var copyMenuItem = new MenuItem { Header = "Copy" };
        
            copyMenuItem.Click += (s, args) => Clipboard.SetText(textBlock.Text);
            contextMenu.Items.Add(copyMenuItem);

            textBlock.ContextMenu = contextMenu;
        }
    }
}