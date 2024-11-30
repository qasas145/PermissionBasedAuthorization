using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class PermissionFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var permission = context.ActionDescriptor.EndpointMetadata.FirstOrDefault(d=>d is PermissionAttribute) as PermissionAttribute;

        var user = context.HttpContext.User;
        
        if (user is not null && permission is not null) {
            
            var canAccess = user.Claims.Any(c=>c.Type == "Permission" && c.Value == permission.Permission);

            if (!canAccess){
                context.Result = new UnauthorizedResult();
            }
        }

    }
}