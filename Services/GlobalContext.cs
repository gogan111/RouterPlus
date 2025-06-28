using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using RouterPlus.Models;

namespace RouterPlus.Services;

public class GlobalContext : INotifyPropertyChanged
{
    private readonly RouterService _routerService;
    public event Action? SmsThreadsReloaded;
    
    private ObservableCollection<SmsThread> _smsThreads = new();
    
    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<SmsThread> SmsThreads
    {
        get => _smsThreads;
        set
        {
            if (_smsThreads == value)
                return;

            _smsThreads = value;
            OnPropertyChanged(nameof(SmsThreads));
        }
    }

    public GlobalContext(RouterService routerService)
    {
        _routerService = routerService;
    }

    public async Task RefreshMessagesAsync()
    { 
        Console.Write("Loading sms list...");
        var smsList = await _routerService.getAllSmsMessages();

        var groupedMessages = smsList
            .GroupBy(sms => sms.Number)
            .Select(group => new SmsThread
            {
                Number = group.Key,
                Messages = group.OrderBy(m => m.Date ?? DateTime.MinValue).ToList(),
            })
            .ToList();

        OverrideSmsThreads(groupedMessages);
        Console.Write("SMS list loaded.");
    }
    
    private void OverrideSmsThreads(List<SmsThread> groupedMessages)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            SmsThreads.Clear();
            groupedMessages.ForEach(m => SmsThreads.Add(m));
            SmsThreadsReloaded?.Invoke();
        });
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}