using DotnetAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable CORS
builder.Services.AddCors((options) =>
{
    options.AddPolicy("DevCors", (corsbuilder) =>
    {
        corsbuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8000")
        .AllowAnyMethod() // allow any Method such as "GET", "POST", etc.
        .AllowAnyHeader() // allow any Header such as "Content-Type", "Authorization", etc.
        .AllowCredentials(); // allow credentials
    });

    options.AddPolicy("ProdCors", (corsbuilder) =>
    {
        corsbuilder.WithOrigins("https://myProductionSite.com") // add your production site here
        .AllowAnyMethod() // allow any Method such as "GET", "POST", etc.
        .AllowAnyHeader() // allow any Header such as "Content-Type", "Authorization", etc.
        .AllowCredentials(); // allow credentials
    });
});

builder.Services.AddScoped<IUserRepository, UserRepository>(); // add builder to access IUserRepository from the Controller

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");  // Ensure CORS middleware is before Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("ProdCors");  // Ensure CORS middleware is before Swagger
    app.UseSwagger();  // You can also add Swagger to Production for testing
    app.UseSwaggerUI();
    app.UseHttpsRedirection();  // Uncomment if needed
}

app.UseAuthorization();  // This should be after the CORS and Swagger middlewares
app.MapControllers();
app.Run();


// if (app.Environment.IsDevelopment())
// {
//     app.UseCors("DevCors"); // CORS should be before Authorization
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }
// else
// {
//     app.UseCors("ProdCors"); // CORS should be before Authorization
//     app.UseHttpsRedirection(); // Uncomment this if HTTPS redirection is needed

// }

// app.UseAuthorization(); // Make sure this comes after CORS


// app.MapControllers();

// app.Run();
