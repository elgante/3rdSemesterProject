using BlazorServer.Auth;
using Domain.Auth;
using HttpClients.ClientInterfaces;
using HttpClients.Implementations;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped(
    sp => 
        new HttpClient { 
            BaseAddress = new Uri("https://localhost:7252") 
        }
);

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();
AuthorizationPolicies.AddPolicies(builder.Services);
builder.Services.AddScoped<IUserService, UserHttpClient>();
builder.Services.AddScoped<IHomeworkService, HomeworkHttpClient>();
builder.Services.AddScoped<IClassService, ClassHttpClient>();
builder.Services.AddScoped<ILessonService, LessonHttpClient>();
builder.Services.AddScoped<IHandInHomeworkService, HandInHomeworkHttpClient>();
builder.Services.AddScoped<IFeedbackService, FeedbackHttpClient>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();