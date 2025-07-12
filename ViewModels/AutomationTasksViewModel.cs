using System.Collections.ObjectModel;
using System.Windows.Input;
using RouterPlus.Core;
using RouterPlus.Models;
using RouterPlus.Models.steps;
using RouterPlus.Services;

namespace RouterPlus.ViewModels;

public class AutomationTasksViewModel : ViewModelBase
{
    private readonly RouterService _routerService;
    private readonly GlobalContext _context;

    List<Step> list = new()
    {
        new FindMessageStep(null, "Find. Hello World!", "number"),
        new SendMessageStep(null, "Send. Hello World!", "number"),
    };
    
    public List<string> AvailableStepTypes { get; } = ["Find Message", "Send Message"];
    public string SelectedStepType { get; set; }
    public ObservableCollection<ScheduledTask> Tasks { get; } = new();
    private ScheduledTask? _selectedTask;
    public ScheduledTask? SelectedTask
    {
        get => _selectedTask;
        set
        {
            if (value != null)
            {
                _selectedTask = value;
                OnPropertyChanged();
            }
        }
    }

    private Step? _selectedStep;

    public Step? SelectedStep
    {
        get => _selectedStep;
        set
        {
            _selectedStep = value;
            OnPropertyChanged(nameof(SelectedStep));
        }
    }

    public ICommand ToggleEditStepCommand { get; }
    public ICommand DeleteStepCommand { get; }
    public ICommand AddStepCommand { get; }
    public ICommand AddTaskCommand { get; }
    public ICommand DeleteTaskCommand { get; }

    public AutomationTasksViewModel(GlobalContext context, RouterService routerService)
    {
        _context = context;
        _routerService = routerService;
        ToggleEditStepCommand = new RelayCommand(ToggleEditStep);
        DeleteStepCommand = new RelayCommand(DeleteStep);
        AddStepCommand = new RelayCommand(AddStep);
        AddTaskCommand = new RelayCommand(AddTask);
        DeleteTaskCommand = new RelayCommand(DeleteTask);
        Tasks.Add(new("Test", list, "* * * * *"));
    }

    private void ToggleEditStep(Object element)
    {
        if (element is Step step)
        {
            step.IsExpanded = !step.IsExpanded;
            Console.WriteLine($"Editing step: {step.IsExpanded}");

            SelectedStep = SelectedStep == step ? null : step;
        }
    }

    private void DeleteStep(Object step)
    {
        SelectedTask?.RemoveStep((Step)step);
    }
    
    private void AddStep(Object o)
    {
        if (SelectedTask == null || string.IsNullOrEmpty(SelectedStepType))
            return;

        Step newStep = SelectedStepType switch
        {
            "Find Message" => new FindMessageStep(_context, "", ""),     
            "Send Message" => new SendMessageStep(_routerService, "", ""),
            _ => throw new ArgumentOutOfRangeException()
        };

        SelectedTask.Steps.Add(newStep);
    }    
    
    private void AddTask(Object o)
    {
        var newTask = new ScheduledTask();
        Tasks.Add(newTask);
        SelectedTask = newTask;
    }
    
    private void DeleteTask(Object o)
    {
        if (SelectedTask == null)
        {
            return;
        }

        Tasks.Remove(SelectedTask);
    }
}