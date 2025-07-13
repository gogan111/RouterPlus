using RouterPlus.Services;
using static RouterPlus.Models.steps.StepType;

namespace RouterPlus.Models.steps;

public class SendMessageStep : Step
{
    public RouterService RouterService { get; set; }

    public SendMessageStep() : base(null, null)
    {
    }

    public SendMessageStep(RouterService routerService, string message, string number)
        : base(message, number)
    {
        RouterService = routerService;
    }

    public override StepType StepType()
    {
        return SendMessage;
    }

    public override bool Execute()
    {
        if (string.IsNullOrEmpty(Number) && string.IsNullOrEmpty(Message))
        {
            return false;
        }
        return RouterService.sendMessage(Message, Number).GetAwaiter().GetResult();
    }
}