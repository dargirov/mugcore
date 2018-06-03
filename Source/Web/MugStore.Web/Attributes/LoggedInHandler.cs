using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace MugStore.Web.Attributes
{
    public class LoggedInHandler : AuthorizationHandler<LoggedInRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public LoggedInHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LoggedInRequirement requirement)
        {
            var loggedin = httpContextAccessor.HttpContext?.Session?.GetString("logged_in");

            if (loggedin != null && loggedin.ToLower() == "true")
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
