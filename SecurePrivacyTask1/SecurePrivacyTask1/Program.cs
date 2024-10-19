using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SecurePrivacyTask1.Models;
using SecurePrivacyTask1.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure SPA static files (served from "ClientApp/dist")
builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = "ClientApp/dist";
});

// Add services to the container
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
    builder => builder.WithOrigins("http://localhost:4200")  // Allow Angular app's origin
                        .AllowAnyMethod()  // Allow any HTTP method (GET, POST, etc.)
                        .AllowAnyHeader()  // Allow any headers
                        .AllowCredentials());  // Allow credentials (optional, based on your auth setup)
});

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Seed MongoDB during startup
using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
    var seed = new MongoDBSeed(database);
    await seed.SeedAsync(); // Ensure the seeding process is awaited
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAngularApp");

//app.UseSpa(spa =>
//{
//    spa.Options.SourcePath = "ClientApp";

//    // If in development, use Angular CLI server
//    if (app.Environment.IsDevelopment())
//    {
//        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
//    }
//});

app.Run();
