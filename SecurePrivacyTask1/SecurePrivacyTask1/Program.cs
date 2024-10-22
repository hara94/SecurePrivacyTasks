using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SecurePrivacyTask1.Models;
using SecurePrivacyTask1.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure SPA static files (served from "ClientApp/dist")
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/dist";
});

// Add CORS policy for Angular app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
    builder => builder.WithOrigins("http://localhost:4200")  // Allow Angular app's origin
                        .AllowAnyMethod()  // Allow any HTTP method (GET, POST, etc.)
                        .AllowAnyHeader()  // Allow any headers
                        .AllowCredentials());  // Allow credentials
});

// Configure MongoDB settings and repository
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddScoped(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return client.GetDatabase(settings.DatabaseName);
});

// Register IMongoRepository<User> with the collection name "Users"
builder.Services.AddScoped<IMongoRepository<User>>(sp =>
{
    var database = sp.GetRequiredService<IMongoDatabase>();
    return new MongoRepository<User>(database, "Users"); // Pass the collection name
});

// Add session handling
builder.Services.AddDistributedMemoryCache(); // Store session in memory
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Secure policy for HTTPS
    options.Cookie.SameSite = SameSiteMode.None;  // This is critical for cross-origin requests
});

// Add authentication services with a default scheme
builder.Services.AddAuthentication("MyCookieAuthenticationScheme")
    .AddCookie("MyCookieAuthenticationScheme", options =>
    {
        options.LoginPath = "/login";  // Path to login page
        options.AccessDeniedPath = "/access-denied";  // Path for access denied
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Secure policy for HTTPS
        options.Cookie.SameSite = SameSiteMode.None;  // This is important for cross-origin requests
    });

builder.Services.AddAuthorization(); // Add Authorization services

var app = builder.Build();

// Seed MongoDB during startup
using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
    var seed = new MongoDBSeed(database);
    await seed.SeedAsync(); // Ensure the seeding process is awaited
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Serve static files for the SPA (Angular app)
app.UseSpaStaticFiles();


// Apply CORS policy
app.UseCors("AllowAngularApp");

// Add session and authentication middleware
app.UseSession();  // UseSession comes before UseAuthentication
app.UseAuthentication(); // Ensure authentication is used before authorization
app.UseAuthorization();  // Ensure authorization middleware is invoked


// Map controllers
app.MapControllers();

app.Run();
