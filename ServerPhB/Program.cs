using Microsoft.EntityFrameworkCore;
using ServerPhB.Configurations;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using ServerPhB.Data;
using ServerPhB.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration);

// Register application services
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<AuthenticationService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Configure Kestrel to listen on specific ports and interfaces
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5254); // Listen on port 5254 for HTTP
    //options.ListenAnyIP(7187, listenOptions => listenOptions.UseHttps()); // Listen on port 7187 for HTTPS
});

// Configure Entity Framework and PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultDatabaseConnection")));

var app = builder.Build();

// Use CORS
app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseRouting();
app.MapControllers();

app.Run();