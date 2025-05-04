using Microsoft.EntityFrameworkCore;
using Serilog;
using Usermanager.Logger;
using Usermanager.Middleware;
using Usermanager.Model.DBContext;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.File(
        "Logs/log-.txt",
        outputTemplate: "{Message:lj}{NewLine}\n",
        rollingInterval: RollingInterval.Day
    )
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5001",    // React/Vue dev server,
                "http://localhost:5002"// Your production domain
              )
              .AllowAnyHeader()
              .AllowAnyMethod();

        // Only add this if you need cookies/auth headers
        // .AllowCredentials();
    });
});

builder.WebHost.UseUrls("http://localhost:5002","https://localhost:5000");


builder.Services.AddScoped<Usermanager.Interfaces.ILocationService, Usermanager.Services.LocationService>();
builder.Services.AddScoped<Usermanager.Interfaces.IUserService, Usermanager.Services.UserService>();
builder.Services.AddScoped<Usermanager.Interfaces.IDepartmentService, Usermanager.Services.DepartmentService>();
builder.Services.AddScoped<Usermanager.Interfaces.IGroupService, Usermanager.Services.GroupService>();
builder.Services.AddScoped<Usermanager.Interfaces.IRoleService, Usermanager.Services.RoleService>();
builder.Services.AddScoped<Usermanager.Interfaces.IAccessibilityService, Usermanager.Services.AccessibilityService>();

builder.Services.AddScoped<ExceptionMiddleware>();
builder.Services.AddScoped<RequestLoggingMiddleware>();


builder.Host.UseSerilog();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();


app.UseCors("AllowMyFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
