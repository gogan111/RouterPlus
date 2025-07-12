using RouterPlus.Models;

namespace RouterPlus.Services;

public class SimpleScheduler : IDisposable
{
    private readonly Dictionary<string, ScheduledTask> _tasks = new();
    private readonly object _lock = new();

    public void AddOrUpdate(ScheduledTask scheduledTask)
    {
        lock (_lock)
        {
            if (_tasks.TryGetValue(scheduledTask.Name, out var existing))
            {
                existing.Dispose();
            }

            _tasks[scheduledTask.Name] = scheduledTask;
            scheduledTask.Start();
        }
    }

    public void Remove(string name)
    {
        lock (_lock)
        {
            if (_tasks.TryGetValue(name, out var task))
            {
                task.Dispose();
                _tasks.Remove(name);
            }
        }
    }

    public void Dispose()
    {
        lock (_lock)
        {
            foreach (var task in _tasks.Values)
            {
                task.Dispose();
            }

            _tasks.Clear();
        }
    }
}