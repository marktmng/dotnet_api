using System.Text;
using DotnetAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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

string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;

// creating authentication token validation parameters
// pull out token key string
SymmetricSecurityKey tokenKey = new SymmetricSecurityKey( // symmetric key and passing to new symmetric key 
    Encoding.UTF8.GetBytes( // and passing to new symmetric key byte array
        tokenKeyString != null ? tokenKeyString : ""
        )
    );

// token validation parameters for application how to use it
TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
{
    IssuerSigningKey = tokenKey,
    ValidateIssuer = false,
    ValidateIssuerSigningKey = false,
    ValidateAudience = false
};

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // set authentication scheme
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = tokenValidationParameters;
    });

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
app.UseAuthentication(); // authentication should be before authorization
app.UseAuthorization();  // This should be after the CORS and Swagger middlewares
// app.UseAuthentication();
app.MapControllers();
app.Run();

