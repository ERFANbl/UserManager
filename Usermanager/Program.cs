using Microsoft.EntityFrameworkCore;
using Usermanager.Model.DBContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyFrontend", policy =>
    {
        // List ALL your frontend URLs here
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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowMyFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
