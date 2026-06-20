using System.Security.Claims;

namespace WatchDiary.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id is null)
            throw new InvalidOperationException("No NameIdentifier claim present on the current user.");
        return int.Parse(id);
    }
}
