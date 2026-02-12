using Serilog; // Import Serilog namespace


// 1. Configure Serilog immediately (before building the host)
// This captures startup errors and sets up where logs should go.
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // Set minimum log level (Debug, Info, Warning, Error...)
    .WriteTo.Console()    // Sink 1: Output logs to the console window
    .WriteTo.File("Logs/cityinfo.txt", rollingInterval: RollingInterval.Day) // Sink 2: Output to file, creating a new file daily
    .CreateLogger(); // Build the logger instance


var builder = WebApplication.CreateBuilder(args);



// 2. Add services to the dependency injection container
// Use Serilog as the logging provider for the host, replacing the default logger
builder.Host.UseSerilog();





// Previously used for built-in ASP.NET Core logging (replaced by Serilog):
// builder.Logging.ClearProviders(); // Cleared default providers like Console, Debug
// builder.Logging.AddConsole();     // Added only Console logging back

// Add controllers support and enable NewtonsoftJson (needed for Patch requests)
builder.Services.AddControllers().AddNewtonsoftJson();




// Add support for standard problem details format (RFC 7807) for errors
builder.Services.AddProblemDetails();




// Add Swagger services to generate API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();



// 3. Configure the HTTP request pipeline (Middleware)
// In non-development environments, use a global exception handler
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}




// In development, enable Swagger UI for testing API endpoints
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS

app.UseRouting(); // Enable routing to match endpoints

app.UseAuthorization(); // Enable authorization middleware

app.MapControllers(); // Map attribute-routed controllers

app.Run(); // Start the application
