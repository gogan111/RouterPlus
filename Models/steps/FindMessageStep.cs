using RouterPlus.Core;
using RouterPlus.Services;
using static RouterPlus.Models.steps.StepType;

namespace RouterPlus.Models.steps;

public class FindMessageStep(GlobalContext context, string message, string number) : Step(message, number)
{
    public override StepType StepType()
    {
        return FindMessage;
    }

    public override bool Execute()
    {
        foreach (var smsThread in context.SmsThreads)
        {
            if (number != null && number != smsThread.Number)
            {
                continue;
            }

            if (message != null && message == smsThread.LastMessage?.Content)
            {
                return true;
            }
        }

        return false;
    }
}