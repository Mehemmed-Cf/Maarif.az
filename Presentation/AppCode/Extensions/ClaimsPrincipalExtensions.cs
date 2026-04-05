using System.Security.Claims;

namespace Presentation.AppCode.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetRequiredUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(id) || !int.TryParse(id, out var uid))
                throw new InvalidOperationException("Identity user id (NameIdentifier) is missing or invalid.");
            return uid;
        }
    }
}
