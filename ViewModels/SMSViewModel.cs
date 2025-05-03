using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using RouterPlus.Core;
using RouterPlus.Models;
using RouterPlus.Services;
using Timer = System.Timers.Timer;

namespace RouterPlus.ViewModels;

public class SmsViewModel : ViewModelBase
{
    private readonly RouterService _routerService;
    private readonly Timer _smsReloadTimer;
    
    private string _newSmsMessage;

    public string NewSmsMessage
    {
        get => _newSmsMessage;
        set
        {
            _newSmsMessage = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<SmsThread> SmsThreads { get; } = [];
    private SmsThread? _selectedThread;

    public SmsThread? SelectedThread
    {
        get => _selectedThread;
        set
        {
            _selectedThread = value;
            OnPropertyChanged();
            LoadSmsThreadAsync();
        }
    }

    //todo replace ICommand with CommunityToolkit.Mvvm ICommand 
    public ICommand LoadMessagesCommand { get; }
    public ICommand SendMessageCommand { get; }

    public SmsViewModel(RouterService routerService)
    {
        _routerService = routerService;
        LoadMessagesCommand = new AsyncRelayCommand(async () => await LoadSmsListAsync());
        SendMessageCommand = new AsyncRelayCommand(async () => await SendMessage());
        
        _smsReloadTimer = new Timer(60000);
        _smsReloadTimer.Elapsed += async (s, e) => await LoadSmsListAsync();
        _smsReloadTimer.AutoReset = true;
        _smsReloadTimer.Start();
    }

    public async Task LoadSmsListAsync()
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
            var currentThreadNumber = SelectedThread?.Number;
            SmsThreads.Clear();

            foreach (var thread in groupedMessages)
            {
                if (thread.Number == currentThreadNumber)
                {
                    SelectedThread = thread;
                }
                SmsThreads.Add(thread);
            }
        });
    }

    private async Task LoadSmsThreadAsync()
    {
        if (SelectedThread != null)
        {
            SelectedThread.Messages = await Task.FromResult(
                SelectedThread.Messages.OrderBy(m => m.Date).ToList()
            );
            OnPropertyChanged(nameof(SelectedThread.Messages));
        }
    }

    private async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(NewSmsMessage)) return;
        if (SelectedThread == null) return;

        if (await _routerService.sendMessage(NewSmsMessage, SelectedThread.Number))
        {
            NewSmsMessage = "";
            //message appears in router not immediately and require some delay before reading
            await Task.Delay(1000);
            await LoadSmsListAsync();
        }
        
        Console.Write("Message sent.");
    }
    
    public void Dispose()
    {
        _smsReloadTimer?.Stop();
        _smsReloadTimer?.Dispose();
    }
}