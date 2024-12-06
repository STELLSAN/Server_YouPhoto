using ServerPhB.Configurations;
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
builder.Services.AddScoped<SocketService>();
builder.Services.AddScoped<AuthenticationService>();

// Configure Kestrel to listen on specific ports and interfaces
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5254); // Listen on port 5254 for HTTP
    options.ListenAnyIP(7187, listenOptions => listenOptions.UseHttps()); // Listen on port 7187 for HTTPS
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();