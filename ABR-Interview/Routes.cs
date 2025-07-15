namespace ABR_Interview;

public static class Routes
{
    public static WebApplication AddRoutes(this WebApplication app)
    {
        app.MapPost("users/{userId}/guest-sessions", async (string userId, GuestSessionService service) =>
        {
            var result = await service.CreateSessionKey(userId);
            if (result is null)
            {
                return Results.BadRequest("Session key already exists.");
            }

            return Results.Ok(result);
        });

        app.MapGet("/users/{userId}/guest-sessions/files", async (
            string userId,
            string sessionKey,
            GuestSessionService guestSessionService,
            FileService fileService) =>
        {
            var isValid = await guestSessionService.ValidateSessionKey(sessionKey, userId);
            if (!isValid)
            {
                return Results.BadRequest("Invalid session key.");
            }

            await guestSessionService.UseSessionKey(sessionKey);
            var files = await fileService.GetAllGuestFiles(userId);

            return Results.Ok(files);
        });

        return app;
    }
}
