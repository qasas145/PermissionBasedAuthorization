using Microsoft.AspNetCore.Authorization;

public class PolicyHandler : AuthorizationHandler<PolicyRequirements>
{

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRequirements requirement)
    {
        var canAccess = context.User.Claims.Any(c=>c.Type == "Permission" && c.Value == requirement.PolicyName);

        if (canAccess) {
            
            context.Succeed(requirement);
            return;
        }

    }
}