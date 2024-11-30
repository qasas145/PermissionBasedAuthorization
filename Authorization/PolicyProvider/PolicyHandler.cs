using Microsoft.AspNetCore.Authorization;

public class PolicyHandler : AuthorizationHandler<PolicyRequirements>
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirements requirement)
    {
        Console.WriteLine("The title is {0}",requirement.PolicyName);
        foreach(var claim in context.User.Claims) {
            Console.WriteLine(claim.Type+' '+claim.Value);
        }
        var canAccess = context.User.Claims.Any(c=>c.Type == "Permission" && c.Value == requirement.PolicyName);
        Console.WriteLine(canAccess);

        if (canAccess) {
            
            context.Succeed(requirement);
            return;
        }

    }
}