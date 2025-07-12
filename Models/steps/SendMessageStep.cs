using RouterPlus.Services;
using static RouterPlus.Models.steps.StepType;

namespace RouterPlus.Models.steps;

public class SendMessageStep(RouterService routerService, string message, string number) : Step(message, number)
{
    public override StepType StepType()
    {
        return SendMessage;
    }

    public override bool Execute()
    {
        return routerService.sendMessage(message, number).GetAwaiter().GetResult();
    }
}