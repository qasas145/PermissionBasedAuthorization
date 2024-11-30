using Microsoft.AspNetCore.Mvc.Filters;

public class PermissionFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var permission = context.ActionDescriptor.EndpointMetadata.FirstOrDefault(d=>d is PermissionAttribute) as PermissionAttribute;
        Console.WriteLine("The permission is {0}", permission.Permission);
    }
}