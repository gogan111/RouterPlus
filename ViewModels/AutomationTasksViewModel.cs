using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using Newtonsoft.Json;
using RouterPlus.Converters;
using RouterPlus.Core;
using RouterPlus.Models;
using RouterPlus.Models.steps;
using RouterPlus.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RouterPlus.ViewModels;

public class AutomationTasksViewModel : ViewModelBase
{
    private const string TasksFilePath = "tasks.json";
    
    private readonly RouterService _routerService;
    private readonly GlobalContext _context;
    
    public List<string> AvailableStepTypes { get; } = ["Find Message", "Send Message"];
    public string SelectedStepType { get; set; }
    public ObservableCollection<ScheduledTask> Tasks { get; set; } = new();
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
    public ICommand SaveTaskCommand { get; }
    public ICommand DeleteTaskCommand { get; }

    public AutomationTasksViewModel(GlobalContext context, RouterService routerService)
    {
        _context = context;
        _routerService = routerService;
        ToggleEditStepCommand = new RelayCommand(ToggleEditStep);
        DeleteStepCommand = new RelayCommand(DeleteStep);
        AddStepCommand = new RelayCommand(AddStep);
        AddTaskCommand = new RelayCommand(AddTask);
        SaveTaskCommand = new RelayCommand(SaveTask);
        DeleteTaskCommand = new RelayCommand(DeleteTask);

        LoadFromDisk(context, routerService);
    }

    private void ToggleEditStep(Object element)
    {
        if (element is Step step)
        {
            step.IsExpanded = !step.IsExpanded;
            SelectedStep = SelectedStep == step ? null : step;
        }
    }

    private void DeleteStep(Object step)
    {
        SelectedTask?.RemoveStep((Step)step);
        SaveToDisk();
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
        SaveToDisk();
    }    
    
    private void AddTask(Object o)
    {
        var newTask = new ScheduledTask();
        Tasks.Add(newTask);
        SelectedTask = newTask;
        SaveToDisk();
    }
    
    private void DeleteTask(Object o)
    {
        if (SelectedTask == null)
        {
            return;
        }

        Tasks.Remove(SelectedTask);
        SaveToDisk();
    }
    
    private void SaveTask(Object o)
    {
        SaveToDisk();
    }
    
    private List<ScheduledTask> LoadTasks(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<ScheduledTask>();
        }

        var json = File.ReadAllText(filePath);

        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.None,
            Converters = { new StepConverter() }
        };

        return JsonConvert.DeserializeObject<List<ScheduledTask>>(json, settings);
    }
    
    private void SaveToDisk()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        string json = JsonSerializer.Serialize(Tasks.ToList(), options);
        File.WriteAllText(TasksFilePath, json);
    }
    
    private void LoadFromDisk(GlobalContext context, RouterService routerService)
    {
        var loadedTasks = LoadTasks(TasksFilePath);
        Tasks = new ObservableCollection<ScheduledTask>(loadedTasks);
        //todo will be fixed soon
        foreach (var task in Tasks)
        {
            foreach (var step in task.Steps)
            {
                switch (step.StepType())
                {
                    case StepType.SendMessage:
                        ((SendMessageStep)step).RouterService = routerService;
                        break;
                    case StepType.FindMessage:
                        ((FindMessageStep)step).Context = context;
                        break;
                }
            }
        }

        OnPropertyChanged(nameof(Tasks));
    }
}