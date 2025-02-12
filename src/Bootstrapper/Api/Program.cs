var builder = WebApplication.CreateBuilder(args);

// Add services to the dependency injection container

builder.Services.AddCarterWithassemblies(typeof(CatalogModule).Assembly);

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

// Register custom services (later)


var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();

// Invoke extension methods for configuring the HTTP request pipeline for each module
app 
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

            
app.Run();
