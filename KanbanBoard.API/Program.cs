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
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddIdentityConfiguration()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
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
