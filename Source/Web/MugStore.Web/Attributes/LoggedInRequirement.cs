using Microsoft.AspNetCore.Authorization;

namespace MugStore.Web.Attributes
{
    public class LoggedInRequirement : IAuthorizationRequirement
    {
    }
}
