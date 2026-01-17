using KanbanBoard.API.Configuration;
using KanbanBoard.API.Middleware;
using KanbanBoard.Application.DependencyInjection;
using KanbanBoard.Infrastructure.DependencyInjections;
using KanbanBoard.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KanbanBoard.Application.IServices;
using KanbanBoard.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure services
// Register Identity first so it doesn't replace the default authentication scheme
builder.Services.AddIdentityConfiguration()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddJwtAuthentication(builder.Configuration);
// For API endpoints, prevent Identity's cookie authentication from redirecting to the
// login page — return 401/403 instead. This avoids 302 responses to SPA/API calls when
// authentication fails and lets the client handle the flow (e.g., refresh token or redirect).
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);
        }
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
        }
        else
        {
            context.Response.Redirect(context.RedirectUri);
        }
        return Task.CompletedTask;
    };
});
builder.Services.AddRateLimitingConfiguration();
builder.Services.AddSecurityHeaders();
builder.Services.AddCorsConfiguration();

//layer services
builder.Services.AddApplication(builder.Configuration); // Application layer DI
builder.Services.AddInfrastructure(builder.Configuration); // Infrastructure layer DI

// core services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
// Current user service (reads user id from the HttpContext claims)
builder.Services.AddScoped<ICurrentUserService, HttpContextCurrentUserService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // Seed roles
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    await SeedRolesAsync(roleManager);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "KanbanBoard API V1");
        c.RoutePrefix = string.Empty;
    });
}

// Apply security headers
app.UseSecurityHeaders(app.Environment);

//global exception handler
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseRateLimiter();

app.UseAuthentication(); // Must come before UseAuthorization
app.UseAuthorization();

app.MapControllers().RequireRateLimiting("GeneralPolicy");

app.Run();

// Seed roles method
async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
{
    string[] roles = { "Admin", "User", "Manager" };
    
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }
    }
}
