using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmergencySituations.Auth
{
    public class AuthFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(!context.HttpContext.Request.Headers.TryGetValue("token", out var tokenKey)) {
                context.Result = new UnauthorizedObjectResult("Need Auth Key");
                return;
            }
            if (!Token.Validation(tokenKey))
            {
                context.Result = new UnauthorizedObjectResult("Invalid Auth Key");
                return;
            }
        }
    }
}
