using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    private readonly GlobalContext _context;

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

    public ObservableCollection<SmsThread> SmsThreads => _context.SmsThreads;
    private SmsThread? _selectedThread;

    public SmsThread? SelectedThread
    {
        get => _selectedThread;
        set
        {
            if (value != null)
            {
                _selectedThread = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand LoadMessagesCommand { get; }
    public ICommand SendMessageCommand { get; }

    public SmsViewModel(GlobalContext context, RouterService routerService)
    {
        _context = context;
        _routerService = routerService;
        _context.SmsThreadsReloaded += OnSmsThreadsReloaded;
        LoadMessagesCommand = new AsyncRelayCommand(async () => await _context.RefreshMessagesAsync());
        SendMessageCommand = new AsyncRelayCommand(async () => await SendMessage());

        //todo delete it 
        _smsReloadTimer = new Timer(60000);
        _smsReloadTimer.Elapsed += async (s, e) => await _context.RefreshMessagesAsync();
        _smsReloadTimer.AutoReset = true;
        _smsReloadTimer.Start();
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
            await _context.RefreshMessagesAsync();
        }

        Console.Write("Message sent.");
    }
    
    private void OnSmsThreadsReloaded()
    {
        TryRestoreSelectedThread();
    }
    
    private void TryRestoreSelectedThread()
    {
        if (_selectedThread == null)
        {
            return;
        }

        var previousNumber = _selectedThread.Number;
    
        SelectedThread = _context.SmsThreads.FirstOrDefault(t => t.Number == previousNumber);
    }
}