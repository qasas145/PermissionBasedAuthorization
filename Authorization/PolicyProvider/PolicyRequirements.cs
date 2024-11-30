using Microsoft.AspNetCore.Authorization;

public class PolicyRequirements : IAuthorizationRequirement {
    public string PolicyName{get;set;}
    public PolicyRequirements(string PolicyName) {
        this.PolicyName = PolicyName;
    }
}