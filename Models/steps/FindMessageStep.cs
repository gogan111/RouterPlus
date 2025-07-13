using RouterPlus.Helpers;
using RouterPlus.Services;
using static RouterPlus.Models.steps.StepType;

namespace RouterPlus.Models.steps;

public class FindMessageStep : Step
{
    public GlobalContext Context { get; set; }

    public FindMessageStep() : base(null, null)
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
        foreach (var thread in Context.SmsThreads)
        {
            if (!String.IsNullOrEmpty(Number) && Number != thread.Number)
            {
                break;
            }

            if (Message == null || thread.LastMessage?.Content == null)
            {
                break;
            }

            if (StringHelper.AreEquivalent(Message, thread.LastMessage.Content))
            {
                Console.WriteLine($"Message found: {thread.LastMessage.Content}");
                return true;
            }
        }

        return false;
    }
}