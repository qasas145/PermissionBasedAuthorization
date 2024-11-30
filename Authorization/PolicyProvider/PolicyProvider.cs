using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

public class PolicyProvider : IAuthorizationPolicyProvider
{
    public DefaultAuthorizationPolicyProvider _policyProvider{get;set;}
    public PolicyProvider(IOptions<AuthorizationOptions> options) {
        _policyProvider = new DefaultAuthorizationPolicyProvider(options);
    }
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return _policyProvider.GetDefaultPolicyAsync();
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return _policyProvider.GetFallbackPolicyAsync();
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith("Permission",StringComparison.OrdinalIgnoreCase)) {
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new PolicyRequirements(policyName));
            return Task.FromResult(policy.Build());
        }
        return _policyProvider.GetPolicyAsync(policyName);
    }
}