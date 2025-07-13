using RouterPlus.Services;
using static RouterPlus.Models.steps.StepType;

namespace RouterPlus.Models.steps;

public class SendMessageStep : Step
{
    public RouterService RouterService { get; set; }

    public SendMessageStep() : base("", "")
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
        return RouterService.sendMessage(Message, Number).GetAwaiter().GetResult();
    }
}