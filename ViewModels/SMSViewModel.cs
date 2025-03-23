using System.Collections.ObjectModel;
using System.Windows.Input;
using RouterPlus.Core;
using RouterPlus.Models;
using RouterPlus.Services;

namespace RouterPlus.ViewModels;

public class SmsViewModel : ViewModelBase
{
    private readonly RouterService _routerService;

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

    public ICommand LoadMessagesCommand { get; }
    public ICommand SendMessageCommand { get; }

    public SmsViewModel(RouterService routerService)
    {
        _routerService = routerService;
        LoadMessagesCommand = new AsyncRelayCommand(async () => await LoadSmsListAsync());
        SendMessageCommand = new RelayCommand(SendMessage);
    }

    public async Task LoadSmsListAsync()
    {
        var smsList = await _routerService.getAllSmsMessages();

        var groupedMessages = smsList
            .GroupBy(sms => sms.Number)
            .Select(group => new SmsThread
            {
                Number = group.Key,
                Messages = group.OrderBy(m => m.Date ?? DateTime.MinValue).ToList(),
            })
            .ToList();

        SmsThreads.Clear();
        foreach (var thread in groupedMessages)
        {
            SmsThreads.Add(thread);
        }
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

    private async void SendMessage(object parameter)
    {
        if (string.IsNullOrWhiteSpace(NewSmsMessage)) return;
        if (SelectedThread == null) return;

        if (await _routerService.sendMessage(NewSmsMessage, SelectedThread.Number))
        {
            NewSmsMessage = "";
        }
    }
}