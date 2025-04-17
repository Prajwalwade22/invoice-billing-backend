using System.Security.Claims;
using InvoiceBillingSystem.Attributes;

namespace InvoiceBillingSystem.Middlewares
{
    public class RoleAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                await _next(context);
                return;
            }

            var requiredRole = endpoint.Metadata.GetMetadata<RequireRoleAttribute>();
            if (requiredRole != null)
            {
                var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value; 

                if (string.IsNullOrEmpty(userRole) || !requiredRole.AllowedRoles.Contains(userRole))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Access Denied: You do not have the required role.");
                    return;
                }
            }

            await _next(context);
        }
    }
}
