using BotSharp.Abstraction.Functions.Models;
using BotSharp.Abstraction.Routing;
using BotSharp.Abstraction.Routing.Settings;

namespace BotSharp.Core.Routing.Handlers;

public class InterruptTaskExecutionRoutingHandler : RoutingHandlerBase, IRoutingHandler
{
    public string Name => "interrupt_task_execution";

    public string Description => "Can't continue user's request becauase the requirements are not met.";

    public List<string> Parameters => new List<string>
    {
        "1. reason: the reason why the request is interrupted",
        "2. answer: the content response to user"
    };

    public bool IsReasoning => true;

    public InterruptTaskExecutionRoutingHandler(IServiceProvider services, ILogger<InterruptTaskExecutionRoutingHandler> logger, RoutingSettings settings) 
        : base(services, logger, settings)
    {
    }

    public async Task<RoleDialogModel> Handle(FunctionCallFromLlm inst)
    {
        var result = new RoleDialogModel(AgentRole.User, inst.Route.Reason)
        {
            FunctionName = inst.Function,
            StopCompletion = true
        };

        return result;
    }
}
