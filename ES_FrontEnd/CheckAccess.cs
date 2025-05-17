using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ES_FrontEnd
{
    public class CheckAccess : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = context.HttpContext.Session.GetString("UserId");
            var role = context.HttpContext.Session.GetString("Role");

            if (userId == null)
            {
                context.Result = new RedirectResult("~/Login");
            }
            else if (role == "Admin")
            {
                // Allow access to Admin area
                if (!context.HttpContext.Request.Path.StartsWithSegments("/Admin"))
                {
                    context.Result = new RedirectResult("~/Admin");
                }
            }
            else if (role == "Customer")
            {
                // Allow access to User panel
                if (context.HttpContext.Request.Path.StartsWithSegments("/Admin"))
                {
                    context.Result = new RedirectResult("~/AccessDenied");
                }
            }
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Expires"] = "-1";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";
            base.OnResultExecuting(context);
        }
    }
}
