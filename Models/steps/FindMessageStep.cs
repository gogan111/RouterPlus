using RouterPlus.Services;
using static RouterPlus.Models.steps.StepType;

namespace RouterPlus.Models.steps;

public class FindMessageStep : Step
{
    public GlobalContext Context { get; set; }

    public FindMessageStep() : base("", "")
    {
    }

    public FindMessageStep(GlobalContext context, string message, string number)
        : base(message, number)
    {
        Context = context;
    }

    public override StepType StepType()
    {
        return FindMessage;
    }

    public override bool Execute()
    {
        foreach (var smsThread in Context.SmsThreads)
        {
            if (Number != null && Number != smsThread.Number)
            {
                continue;
            }

            if (Message != null && Message == smsThread.LastMessage?.Content)
            {
                return true;
            }
        }

        return false;
    }
}