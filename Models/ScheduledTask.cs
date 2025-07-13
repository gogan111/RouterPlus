using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Timer = System.Timers.Timer;

namespace RouterPlus.Models;

public class ScheduledTask : IDisposable, INotifyPropertyChanged
{
    private string _name;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Step> Steps { get; } = new();
    private Timer _timer = new Timer();
    private DateTime _nextRun;

    public ScheduledTask()
    {
        Start();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public void Start()
    {
        //todo delete it 
        _timer = new Timer(20000);
        _timer.Elapsed += async (s, e) => Run();
        _timer.AutoReset = true;
        _timer.Start();
    }

    public void RemoveStep(Step step)
    {
        bool remove = Steps.Remove(step);
        if (remove)
        {
            Console.WriteLine($"Step '{step.Name}' removed.");
        }
    }

    private void Run()
    {
        foreach (var step in Steps)
        {
            var isSuccess = step.Execute();

            if (!isSuccess)
            {
                Console.WriteLine($"Step: '{step.Name}', return status false.");
                return;
            }
        }

        Console.WriteLine($"Task: {Name}, was successfully finished.");
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}