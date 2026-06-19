using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FUManagementSystem.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeRoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly int[] _roles;

        public AuthorizeRoleAttribute(params int[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var roleStr = context.HttpContext.Session.GetString("AccountRole");
            if (roleStr == null)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            if (_roles.Length > 0 && int.TryParse(roleStr, out int role))
            {
                if (!_roles.Contains(role))
                {
                    context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
                    return;
                }
            }
        }
    }
}
