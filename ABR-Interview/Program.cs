using ABR_Interview;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<GuestSessionService>();
builder.Services.AddTransient<FileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("users/{userId}/guest-sessions", async (string userId, GuestSessionService service) =>
{
    var result = await service.CreateSessionKey(userId);
    if(result is null)
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



app.Run();