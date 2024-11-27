var builder = WebApplication.CreateBuilder(args);

// Add services to the dependency injection container


// Register custom services (later)


var app = builder.Build();

// Configure the HTTP request pipeline

app.Run();
