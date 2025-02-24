using System.Web.Mvc;
using System;

namespace NotifyHub.Infrastructure.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.Items["UserId"];
            if (user == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}