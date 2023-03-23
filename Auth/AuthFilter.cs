using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmergencySituations.Auth
{
    public class AuthFilter : Attribute, IAuthorizationFilter
    {
        public List<string> NotAvailableTables = new List<string>();

        public AuthFilter(string[] tables)
        {
            NotAvailableTables = tables.ToList();
        }

        public AuthFilter() { }


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (CheckUrl(context.HttpContext.Request.GetDisplayUrl()))
            {
                if (!context.HttpContext.Request.Headers.TryGetValue("token", out var tokenKey))
                {
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
        private bool CheckUrl(string url)
        {
            return NotAvailableTables.Find(i => i != url.Split('/').ToList().Last()) == null;
        }
    }
}
