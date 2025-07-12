using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NCrontab;

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
    private readonly CrontabSchedule _schedule;
    private Timer _timer;
    private DateTime _nextRun;

    public ScheduledTask()
    {

    }
    
    public ScheduledTask(string name, List<Step> steps, string cronExpression)
    {
        Name = name;
        
        Steps = new ObservableCollection<Step>(steps);
        _schedule = CrontabSchedule.Parse(cronExpression);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public void Start()
    {
        ScheduleNextRun();
    }

    public void RemoveStep(Step step)
    {
        bool remove = Steps.Remove(step);
        if (remove)
        {
            Console.WriteLine($"Step '{step.Name}' removed." );
        }
    }

    private void ScheduleNextRun()
    {
        _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
        var delay = _nextRun - DateTime.Now;

        _timer = new Timer(_ => Run(), null, delay, Timeout.InfiniteTimeSpan);
    }

    private void Run()
    {
        foreach (var step in Steps)
        {
            var completed = step.Execute();

            if (!completed)
            {
                Console.WriteLine($"Step: {step}, return status false.");
                break;
            }
        }
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}