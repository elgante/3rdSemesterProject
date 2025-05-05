using System.Text;
using Application.ClientInterfaces;
using Application.gRPCClients;
using Application.Logic;
using Application.LogicInterfaces;
using Domain.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUserLogic,UserLogic>();
builder.Services.AddScoped<IUserClient,UserClient>();

builder.Services.AddScoped<IHomeworkLogic, HomeworkLogic>();
builder.Services.AddScoped<IHomeworkClient, HomeworkClient>();

builder.Services.AddScoped<ILessonLogic, LessonLogic>();
builder.Services.AddScoped<ILessonClient, LessonClient>();

builder.Services.AddScoped<IClassLogic, ClassLogic>();
builder.Services.AddScoped<IClassClient, ClassClient>();

builder.Services.AddScoped<IHandInHomeworkLogic, HandInHomeworkLogic>();
builder.Services.AddScoped<IHandInHomeworkClient, HandInHomeworkClient>();

builder.Services.AddScoped<IFeedbackLogic, FeedbackLogic>();
builder.Services.AddScoped<IFeedbackClient, FeedbackClient>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
AuthorizationPolicies.AddPolicies(builder.Services);

builder.Services.AddLogging(builder => builder
    .AddConsole()
);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Add JWT authentication to Swagger
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter your JWT token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    // Make sure Swagger UI requires a Bearer token specified
    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securityScheme, new[] { "Bearer" } }
    };

    c.AddSecurityRequirement(securityRequirement);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");

        // Add a button for JWT authorization in the Swagger UI
        c.OAuthClientId("swagger");
        c.OAuthAppName("Swagger UI");
    });
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();