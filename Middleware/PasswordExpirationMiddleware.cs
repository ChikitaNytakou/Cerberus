using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

public class PasswordExpirationMiddleware
{
    private readonly RequestDelegate _next;

    public PasswordExpirationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, UserManager<User> userManager)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null && user is User appUser)
                {
                    // Проверяем, если LastLoginDate == DateTime.MinValue или меньше текущего времени
                    if (appUser.LastLoginDate == DateTime.MinValue || appUser.DateOfEnd < DateTime.UtcNow)
                    {
                        // Исключаем перенаправление, если пользователь уже на странице смены пароля
                        if (!context.Request.Path.StartsWithSegments("/Account/ChangePassword"))
                        {
                            context.Response.Redirect("/Account/ChangePassword");
                            return;
                        }
                    }
                }
            }
        }

        await _next(context);
    }
}
