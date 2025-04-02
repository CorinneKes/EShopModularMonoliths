
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => 
config.ReadFrom.Configuration(context.Configuration));

// Add services to the dependency injection container

// Common services: Carter, MediatR, FluentValidation
var catalogAssembly = typeof(CatalogModule).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;

builder.Services.AddCarterWithassemblies(catalogAssembly, basketAssembly);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(catalogAssembly, basketAssembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssemblies([catalogAssembly, basketAssembly]);


// Module-specific services

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Register custom services (later)


var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(options => { });

// Invoke extension methods for configuring the HTTP request pipeline for each module
app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();


app.Run();
